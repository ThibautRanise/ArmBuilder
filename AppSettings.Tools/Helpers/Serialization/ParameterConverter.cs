using AppSettings.Tools.Models.ARM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AppSettings.Tools.Helpers.Serialization
{
    public class ParameterConverter : JsonConverter<Parameter>
    {
        private const string _allowedValuesKey = "allowedValues";
        private const string _defaultValue = "defaultValue";
        private const string _type = "type";

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create an instance of IParameter depending on parameter type definied in the arm template
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="jObject"></param>
        /// <returns></returns>
        protected override Parameter Create(Type objectType, JObject jObject)
        {
            var type = jObject[_type].ToString();
            var allowedValues = jObject[_allowedValuesKey];
            var defaultValue = jObject[_defaultValue];

            switch (type)
            {
                case "string" :
                    return new ParameterClass<string>()
                    {
                        AllowedValues = (allowedValues != null && allowedValues.HasValues) ? new List<string>(allowedValues?.Values<string>()) : null,
                        DefaultValue = defaultValue?.Value<string>()
                    };

                case "int":
                    var pInt = new ParameterStruct<int>("int");

                    if (allowedValues != null && allowedValues.HasValues)
                        pInt.AllowedValues =  new List<int>(allowedValues.Values<int>());

                    if (defaultValue != null && defaultValue.HasValues)
                        pInt.DefaultValue = defaultValue.Value<int>();

                    return pInt;
            }

            return new Parameter();
        }
    }
}
