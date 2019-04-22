using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace XUnitTestCommon.TestCreator
{
   public class SwaggerJsonModel
   {
        public static List<RequestModel> CreateApiModels(string swaggerRootJson)
        {
            var apiMethodsModel = JObject.Parse(swaggerRootJson).ToObject<ApiMethodModel>();

            var result = new List<RequestModel>();
            JObject swaggerJobject = JObject.Parse(swaggerRootJson);
            var objs = swaggerJobject["paths"].ToList();

            foreach (var o in objs)
            {
                JProperty p = o.ToObject<JProperty>();

                var tempModel = new RequestModel
                {
                    RequestModelURL = p.Name,
                    ApiMethods = new List<ApiMethodModel>()
                };

                var pChildren = p.Values().ToList();

                foreach (var t in pChildren)
                {
                    var d = t.ToObject<JProperty>();

                    var c = d.Value.ToObject<ApiMethodModel>();
                    c.ApiMethod = d.Name;

                    tempModel.ApiMethods.Add(c);
                }

                result.Add(tempModel);
            }

            return result;
        }

        //public static Dictionary<string, List<string>> GetTestNames(RequestModel requestModel)
        //{
        //    var testNames = new Dictionary<string, List<string>>();
        //    var list = new List<string>();

        //    var urlPathes = requestModel.RequestModelURL.Replace("{", "").Replace("}", "").Split('/').ToList().FindAll(p => p.Length > 0);

        //    var namePattern = "";
        //    urlPathes.ToList().ForEach(a =>
        //    {
        //        namePattern = namePattern + $"{a.First().ToString().ToUpper() + a.Substring(1)}";
        //    });

        //    requestModel.ApiMethods.ForEach(m =>
        //    {
        //        testNames[namePattern] = new List<string>();

        //        var baseName = $"{m.ApiMethod.First().ToString().ToUpper() + m.ApiMethod.Substring(1)}{namePattern}";

        //        testNames[namePattern].Add(baseName);

        //        var notNullRespo = m.Responses.GetResponses();
        //        notNullRespo.ForEach(n => testNames[namePattern].Add($"{baseName}{n}ResponseTest"));
        //    });

        //    return testNames;
        //}

        //public static Dictionary<string, List<TestModel>> GetTests(RequestModel requestModel)
        //{
        //    var testNames = new Dictionary<string, List<TestModel>>();
        //    var list = new List<string>();

        //    var urlPathes = requestModel.RequestModelURL.Replace("{", "").Replace("}", "").Split('/').ToList().FindAll(p => p.Length > 0);

        //    var namePattern = "";
        //    urlPathes.ToList().ForEach(a =>
        //    {
        //        namePattern = namePattern + $"{a.First().ToString().ToUpper() + a.Substring(1)}";
        //    });

        //    requestModel.ApiMethods.ForEach(m =>
        //    {
        //        testNames[namePattern] = new List<TestModel>();

        //        var baseName = $"{m.ApiMethod.First().ToString().ToUpper() + m.ApiMethod.Substring(1)}{namePattern}()";

        //        testNames[namePattern].Add( new TestModel { TestName = baseName, Category = "something from env/CLI", Description = requestModel.summary });

        //        var notNullRespo = m.Responses.GetResponses();
        //        notNullRespo.ForEach(n => testNames[namePattern].Add(
        //            new TestModel { TestName = $"{baseName}{n}ResponseTest", Category = "something from env/CLI", Description = requestModel.summary }
        //            )
        //            );
        //    });

        //    return testNames;
        //}

        public static SwaggerJson GetTests(RequestModel requestModel)
        {
            var testNames = new SwaggerJson();
            testNames.TestsContainers = new List<TestsContainer>();
            var list = new List<string>();

            var urlPathes = requestModel.RequestModelURL.Replace("{", "").Replace("}", "").Split('/').ToList().FindAll(p => p.Length > 0);

            var namePattern = "";
            urlPathes.ToList().ForEach(a =>
            {
                namePattern = namePattern + $"{a.First().ToString().ToUpper() + a.Substring(1)}";
            });

            requestModel.ApiMethods.ForEach(m =>
            {
                var testContainer = new TestsContainer();
                testContainer.ApiPath = namePattern;
                testContainer.TestModels = new List<TestModel>();

                var baseName = $"{m.ApiMethod.First().ToString().ToUpper() + m.ApiMethod.Substring(1)}{namePattern}";

                testContainer.TestModels.Add(new TestModel { TestName = $"{baseName}()", Category = "something from env/CLI", Description = requestModel.summary });

                var notNullRespo = m.Responses.GetResponses();
                notNullRespo.ForEach(n => testContainer.TestModels.Add(
                    new TestModel { TestName = $"{baseName}{n}ResponseTest()", Category = "something from env/CLI", Description = requestModel.summary }
                    ));

                testNames.TestsContainers.Add(testContainer);
            });

            return testNames;
        }
    }
}
