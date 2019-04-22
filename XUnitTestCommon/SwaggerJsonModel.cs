using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using XUnitTestCommon.TestCreator;

namespace XUnitTestCommon
{
    public class SwaggerJsonModel1
    {
        //public static string JSON { get { return File.ReadAllText(@"c:\Lykke\Lykke.Automation.Tests\XUnitTestCommon\swaggerJson.txt"); } }
        //C:\Users\Yasonau\Desktop\auto.json
        //public static string JSON { get { return File.ReadAllText(@"C:\Users\Yasonau\Desktop\auto.json"); } }
        //public static string JSON { get { return File.ReadAllText(@"C:\Users\Yasonau\Desktop\auto1.json"); } }
        public static string JSON { get { return File.ReadAllText(@"C:\Users\Yasonau\Desktop\auto2.json"); } }
        public static List<string> methods { get { return new List<string> { "get", "post", "put", "delete" }; } }

        JObject ob = JObject.Parse(JSON);

       // [Test]
        public void someTest1()
        {
            var swaggerModels = SwaggerJsonModel.CreateApiModels(JSON);


            swaggerModels.ForEach(model => SwaggerJsonModel.GetTests(model).CreateTests());
     
        }       
    }
}

