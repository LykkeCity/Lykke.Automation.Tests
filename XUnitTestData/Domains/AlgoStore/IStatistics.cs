using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IStatisticss : IDictionaryItem
    {
         bool IsBuy { get; set; }
         decimal Price { get; set; }
         int Amount { get; set; }
    }
}
