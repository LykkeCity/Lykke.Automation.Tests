using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IMetaData : IDictionaryItem
    {
         string Name { get; set; }
         string Description { get; set; }
    }
}
