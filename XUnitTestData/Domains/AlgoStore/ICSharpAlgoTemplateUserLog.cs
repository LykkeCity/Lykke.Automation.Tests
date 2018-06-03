using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface ICSharpAlgoTemplateUserLog : IDictionaryItem
    {
        string DateTime { get; set; }
        string Message { get; set; }
    }
}