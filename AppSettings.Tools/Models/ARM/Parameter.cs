using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppSettings.Tools.Models.ARM
{
    public class ParameterStruct<T> : Parameter
        where T : struct
    {
        public ParameterStruct(string typeName = "")
        {
            if (!string.IsNullOrEmpty(typeName))
                base.Type = typeName;
            else
                base.Type = typeof(T).Name;
        }

        [JsonProperty(PropertyName = "allowedValues")]
        public List<T> AllowedValues { get; set; }

        [JsonProperty(PropertyName = "defaultValue")]
        public T? DefaultValue { get; set; }
    }

    public class ParameterClass<T> : Parameter
    where T : class
    {
        public ParameterClass(string typeName = "")
        {
            if (!string.IsNullOrEmpty(typeName))
                base.Type = typeName;
            else
                base.Type = typeof(T).Name;
        }

        [JsonProperty(PropertyName = "allowedValues")]
        public List<T> AllowedValues { get; set; }

        [JsonProperty(PropertyName = "defaultValue")]
        public T DefaultValue { get; set; }
    }


    public class Parameter : IParameter
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    public interface IParameter
    {
        string Type { get; set; }
    }
}
