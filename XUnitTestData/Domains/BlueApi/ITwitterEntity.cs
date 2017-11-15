using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.BlueApi
{
    interface ITwitterEntity: IDictionaryItem
    {
        bool IsExtendedSearch { get; set; }
        string AccountEmail { get; set; }
        string SearchQuery { get; set; }
        int MaxResult { get; set; }
        DateTime UntilDate { get; set; }
        int PageSize { get; set; }
        int PageNumber { get; set; }
    }
}
