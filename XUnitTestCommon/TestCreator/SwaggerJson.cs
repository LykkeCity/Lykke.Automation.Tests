using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XUnitTestCommon.TestCreator
{
    public class SwaggerJson
    {
        public List<TestsContainer> TestsContainers { get; set; }

        public void CreateTests()
        {
            TestsContainers.ForEach(test => File.WriteAllText($@"C:\test\NewModels\{test.ApiPath}.cs", test.ClassRepresentation));
        }
    }
}
