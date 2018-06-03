namespace XUnitTestData.Domains.AlgoStore
{
    public interface IAlgoStoreApiLog : IDictionaryItem
    {
         string DateTime { get; set; }
         string Level { get; set; }
         string AppName { get; set; }
         string Version { get; set; }
         string Component { get; set; }
         string Process { get; set; }
         string Context { get; set; }
         string Msg { get; set; }
         string Type { get; set; }
         string Stack { get; set; }
    }
}