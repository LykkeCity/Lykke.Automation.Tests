using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IAlgoRatingsTable : IDictionaryItem
    {
        int Rating { get; set; }
    }
}
