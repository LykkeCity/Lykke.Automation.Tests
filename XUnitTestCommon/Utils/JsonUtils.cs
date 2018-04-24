using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string SerializeObject(object model)
        {
            return JsonConvert.SerializeObject(model, Formatting.Indented);
        }
    }
}
