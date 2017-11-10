using System;

namespace XUnitTestData.Domains.ApiV2
{
    public interface IOperationDetails : IDictionaryItem
    {
        string ClientId { get; set; }
        string Comment { get; set; }
        DateTime CreatedAt { get; set; }
        string TransactionId { get; set; }
    }
}