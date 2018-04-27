using AppSettings.Tools.Builders;
using System;
using System.Collections.Generic;

namespace AppSettings.Tools
{
    class Program
    {
        static void Main(string[] args)
        {
            string armTemplatePath = @"C:\*********";
            string projectUrl = @"C:\*******";
            string env = "stg";

            // Define each web applications with corresponding path to the appSettings.json folder
            Dictionary<string, string> webAppsWithConf = new Dictionary<string, string>()
            {
                { "api", $"{projectUrl}\api" },
                { "back", $"{projectUrl}\back" },
                { "front", $"{projectUrl}\front" }
            };

            // Load the builder with the arm template path
            var armBuilder = new ArmTemplateBuilder(armTemplatePath);
            
            foreach(var webApp in webAppsWithConf)
            {
                armBuilder
                    .EnrichAppService(webApp.Key)
                    .WithConf(webApp.Value, env);
            }

            // Enrich an return arm template with appSettings.json keys & values for each web app defined in [webAppsWithConf] dictionary
            var enrichArmTemplate = armBuilder.Build();

            Console.Read();
        }
    }
}