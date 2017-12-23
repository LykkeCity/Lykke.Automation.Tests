using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AFTests.PrivateApiTests
{
    public static class Helpers
    {
        public static void AreEqualByJson(object expected, object actual)
        {
            var expectedJson = JsonConvert.SerializeObject(expected);
            var actualJson = JsonConvert.SerializeObject(actual);
            Assert.That(expectedJson, Is.EqualTo(actualJson), "Objects are not equals");
        }
    }
}
