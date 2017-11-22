﻿using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestCommon.Reports
{
    public class AllureCategories
    {
        public string GetJson()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            return JsonConvert.SerializeObject(GetCategories(), settings);
        }

        //custom categories
        private List<AllureCategory> GetCategories()
        {
            return new List<AllureCategory>
            {
                new AllureCategory
                {
                    name = "Ignored tests",
                    messageRegex = ".*",
                    matchedStatuses = new List<AllureStatuses> { AllureStatuses.skipped }
                }
            };
        }

        private class AllureCategory
        {
            public string name { get; set; }
            public string messageRegex { get; set; }
            public string traceRegex { get; set; }
            public List<AllureStatuses> matchedStatuses { get; set; }
        }

        private enum AllureStatuses { skipped, broken, failed, passed }
    }
}
