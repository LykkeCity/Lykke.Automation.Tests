using System;

namespace XUnitTestData.Domains.ApiV2
{
    public interface IOperations : IDictionaryItem
    {
        Guid ClientId { get; set; }
        string Context { get; set; }
        DateTime Created { get; set; }
        string PrimaryPartitionKey { get; set; }
        string PrimaryRowKey { get; set; }
        string StatusString { get; set; }
        string TypeString { get; set; }
    }
}