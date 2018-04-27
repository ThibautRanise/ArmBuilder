using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AppSettings.Tools.Builders
{
    public class AppSettingsBuilder
    {
        private const string _configurationFileName = "appsettings.json";
        private string _path;
        private string _envTarget;

        public AppSettingsBuilder(string path, string envTarget)
        {
            this._path = path;
            this._envTarget = envTarget;
        }

        /// <summary>
        /// Load in a dictionary each keys and values stored in the specified 'appsettings.json' configuration file
        /// </summary>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public Dictionary<string, object> Build(Dictionary<string, object> overrides)
        {
            var json = File.ReadAllText($"{this._path}\\${_configurationFileName}");

            JObject jo = JObject.Parse(json);

            Dictionary<string, object> configurations = new Dictionary<string, object>();

            // 1. Inspect each node from base configuration file and format them
            foreach (var c in jo.Descendants())
            {
                if (!c.Children().Any())                        // Get only the final node level
                {
                    var key = c.Path.Replace(".", ":");
                    string val = c.ToString();
                    configurations.Add(key, val);
                }
            }

            // 2. Inspect each node from specific environnement configuration file and format them
            if (! string.IsNullOrEmpty(this._envTarget))
            {
                var jsonOverridePath = $"{this._path}\\${_configurationFileName}.{this._envTarget}.json";
                var jsonOverride = File.ReadAllText(jsonOverridePath);

                JObject joOverride = JObject.Parse(jsonOverride);
                Dictionary<string, object> datasOverrides = new Dictionary<string, object>();

                foreach (var c in joOverride.Descendants())
                {
                    if (!c.Children().Any())                       
                    {
                        var key = c.Path.Replace(".", ":");
                        string val = c.ToString();
                        datasOverrides.Add(key, val);
                    }
                }

                // 2.1 Override base configurations
                foreach(var d in datasOverrides)
                    configurations[d.Key] = d.Value;
            }

            // 3. Override configurations with specific values defined in [overrides] parameters
            if (overrides != null)
            {
                foreach(var appSettingOveride in overrides)
                {
                    var appSetting = configurations.FirstOrDefault(_ => _.Key == appSettingOveride.Key);

                    if (appSetting.Key != null)
                        configurations[appSetting.Key] = appSettingOveride.Value;
                }
            }

            return configurations;
        }
    }
}
