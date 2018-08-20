using System;
using XUnitTestData.Enums;

namespace XUnitTestData.Domains.AlgoStore
{
    public interface IAlgo : IDictionaryItem
    {
        String AlgoId { get; set; }
        String AlgoMetaDataInformationJSON { get; set; }
        String AlgoVisibilityValue { get; set; }
        String ClientId { get; set; }
        String Description { get; set; }
        String Name { get; set; }
        String Author { get; set; }
        DateTime DateCreated { get; set; }
    }
}
