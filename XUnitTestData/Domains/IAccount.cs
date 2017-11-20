using XUnitTestData.Entities;

namespace XUnitTestData.Domains
{
    public interface IAccount : IDictionaryItem
    {
        string Id { get; }
        string Balances { get; set; }
        BalanceDTO[] BalancesParsed { get; set; }
    }
}