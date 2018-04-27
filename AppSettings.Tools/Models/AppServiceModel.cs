using System.Collections.Generic;

namespace AppSettings.Tools.Models
{
    public class AppServiceModel
    {
        public AppServiceModel(string name)
        {
            this.Name = name;
            this.AppSettings = new Dictionary<string, object>();
            this.AppSettingsOverrides = new Dictionary<string, object>();
        }

        public AppServiceModel WithConf(string appSettingsFolder, string targetEnv = "")
        {
            this.AppSettingsPath = appSettingsFolder;
            this.TargetEnv = targetEnv;
            return this;
        }

        public string TargetEnv { get; set; }

        public string Name { get; set; }

        public string AppSettingsPath { get; set; }

        public Dictionary<string, object> AppSettings { get; set; }

        public Dictionary<string, object> AppSettingsOverrides { get; set; }

        public AppServiceModel OverrideAppSettings(Dictionary<string, object> appSettings)
        {
            foreach (var appS in appSettings)
                this.Override(appS.Key, appS.Value);
            return this;
        }

        private void Override(string key, object value)
        {
            this.AppSettingsOverrides.Add(key, value);
        }
    }
}
