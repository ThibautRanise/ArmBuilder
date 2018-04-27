using AppSettings.Tools.Helpers.Serialization;
using AppSettings.Tools.Models;
using AppSettings.Tools.Models.ARM;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppSettings.Tools.Builders
{
    public class ArmTemplateBuilder
    {
        private string _templatePath;

        public ArmTemplateBuilder(string templatePath, string targetEnv = "")
        {
            this._templatePath = templatePath;
            this._appServices = new List<AppServiceModel>();
        }

        private List<AppServiceModel> _appServices;

        public List<AppServiceModel> AppServices
        {
            get { return _appServices; }
            set { _appServices = value; }
        }

        public AppServiceModel EnrichAppService(string displayName)
        {
            var appService = new AppServiceModel(displayName);
            this._appServices.Add(appService);
            return appService;
        }

        public string Build()
        {
            // 1. Deserialize the ARM template
            string fileContent = File.ReadAllText(this._templatePath);
            Template baseTemplate = JsonConvert.DeserializeObject<Template>(fileContent, new JsonConverter[] { new ParameterConverter() });

            // 2. Find nodes for web app resources in the template object
            var appServices = baseTemplate.Resources.Where(_ => _.Type == ARM.WebAppRessource);

            // 3. Iterate on each web app resources
            foreach (var appService in appServices)
            {
                // 3.1 Check if the current web app is overide by the builder
                var overide = this._appServices.FirstOrDefault(_ => _.Name == appService.DisplayName);

                if (overide != null)
                {
                    // 3.2 Find the appSettings resource
                    var appSettingResource = appService.Resources.FirstOrDefault(_ => _.Name == ARM.AppSettings);

                    // 3.3 Create the appSettings resource if does not exists
                    if (appSettingResource == null)
                    {
                        appService.Resources.Add(new Resource() { Name = ARM.AppSettings, Type = ARM.ConfigResource, ApiVersion = ARM.ConfigResourceApiVersion, DependsOn = new List<string>() { $"[resourceId('Microsoft.Web/sites', {appService.Name.Replace("[", "").Replace("]", "")})]" }, Properties = new Dictionary<string, object>() });
                        appSettingResource = appService.Resources.FirstOrDefault(_ => _.Name == ARM.AppSettings);
                    }

                    // 3.4 Load appSettings from local configuration file
                    var appSettingsFromFile = new AppSettingsBuilder(overide.AppSettingsPath, overide.TargetEnv).Build(overrides: overide.AppSettingsOverrides);

                    // 3.5 Popule the web app appSettings
                    if (appSettingResource == null || !appSettingResource.Properties.Any())
                        appSettingResource.Properties = new Dictionary<string, object>();
                    else
                        foreach (var newAppSetting in appSettingsFromFile)
                        {
                            if (!appSettingResource.Properties.Any(_ => _.Key == newAppSetting.Key))
                                appSettingResource.Properties.Add(newAppSetting.Key, newAppSetting.Value);
                        }
                }
            }

            var existingsResources = baseTemplate.Resources.Where(_ => _.Type != ARM.WebAppRessource).ToList();

            foreach (var newAppService in appServices)
                existingsResources.Add(newAppService);

            baseTemplate.Resources = existingsResources;

            return JsonConvert.SerializeObject(baseTemplate);
        }
    }
}