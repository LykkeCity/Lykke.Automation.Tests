using Newtonsoft.Json;
using System;

namespace XUnitTestCommon.Utils
{
    public class JsonUtils
    {
        public static Object DeserializeJson(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public static T DeserializeJson<T>(string json)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            };

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static string SerializeObject(object model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}
