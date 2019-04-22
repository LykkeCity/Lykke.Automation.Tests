using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.TestCreator
{
    public class TestsContainer
    {
        public string ApiPath { get; set; }
        public List<TestModel> TestModels { get; set; }

        public string ClassRepresentation
        {
            get
            {
                var tests = "";
                var uses = "using NUnit.Framework; \r\nusing System.Net; \r\n";
                var nameSpace = "namespace Api.Tests \r\n{ ";
                TestModels.ForEach(test => tests = $"{tests}{test.StringRepresantation()}");

                var className = $"{uses}{nameSpace}\r\npublic class {ApiPath}\r\n{{{tests} \r\n}}\r\n}}";

                return className;
            }
        }
    }
}
