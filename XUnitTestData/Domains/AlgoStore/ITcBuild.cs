namespace XUnitTestData.Domains.AlgoStore
{
    public interface ITcBuild : IDictionaryItem
    {
        string InstanceId { get; set; }
        string ClientId { get; set; }
        string TcBuildId { get; }
    }
}
