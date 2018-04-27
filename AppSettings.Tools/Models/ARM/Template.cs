using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppSettings.Tools.Models.ARM
{
    public class Template
    {
        [JsonProperty(PropertyName = "$schema")]
        public string Schema { get; set; }

        [JsonProperty(PropertyName = "parameters")]
        public Dictionary<string, Parameter> Parameters { get; set; }

        [JsonProperty(PropertyName = "variables")]
        public Dictionary<string, object> Variables { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<Resource> Resources { get; set; }

        [JsonProperty(PropertyName = "outputs")]
        public Dictionary<string, object> Outputs { get; set; }
    }
}
