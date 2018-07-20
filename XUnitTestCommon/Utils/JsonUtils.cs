using Newtonsoft.Json;
using System;

namespace XUnitTestCommon.Utils
{
    public class JsonUtils
    {
        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static Object DeserializeJson(string json)
        {
            return JsonConvert.DeserializeObject(json, settings);
        }

        public static T DeserializeJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static string SerializeObject(object model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented, settings);
        }
    }
}
