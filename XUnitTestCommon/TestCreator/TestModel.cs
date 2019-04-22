using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.TestCreator
{
    public class TestModel
    {
        public string TestName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public string StringRepresantation()
        {
            string _Test = "\r\n[Test]\r\n";

            Category = string.IsNullOrEmpty(Category) ? "" : $"[Category(\"{Category}\")]\r\n";

            Description = string.IsNullOrEmpty(Description) ? "" : $"[Description(\"{Description}\")]";

            return $"{_Test}{Category}{Description}public void {TestName}{{}}";
        }
    }
}
