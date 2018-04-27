using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AppSettings.Tools.Models.ARM
{

    public class Resource
    {
        public Resource()
        {
            Tags = new Dictionary<string, string>();
        }

        [JsonProperty(PropertyName = "apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonIgnore]
        public string DisplayName => this.Tags?.FirstOrDefault(_ => _.Key == "displayName").Key != null ? this.Tags?.FirstOrDefault(_ => _.Key == "displayName").Value : null;

        [JsonProperty(PropertyName = "dependsOn")]
        public List<string> DependsOn { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public Dictionary<string, object> Properties { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public Dictionary<string, string> Tags { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<Resource> Resources { get; set; }
    }
}
