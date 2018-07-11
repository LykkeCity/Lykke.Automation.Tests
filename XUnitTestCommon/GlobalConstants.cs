using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon
{
    public class GlobalConstants
    {
        public static readonly string AutoTest = "_AutoTest";
        public static readonly string AutoTestEdit = "_AutoTestEdit";
        public static readonly string AutoTestEmail = "_autotest@auto.test";

        public static readonly string GuidRegexPattern = "^[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}$";
        public static readonly string ApiVersionRegexPattern = "^\\d+\\.\\d+\\.\\d+\\.\\d+$";
    }
}
