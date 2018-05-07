using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IStatistics : IDictionaryItem
    {
         int BuildImageId { get; set; }
         long ImageId { get; set; }
    }
}
