using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Entities.AlgoStore
{
    public class TcBuildEntity : TableEntity, ITcBuild
    {
        public string InstanceId { get; set; }
        public string ClientId { get; set; }
        public string TcBuildId => Id;
        public string Id => RowKey;
    }
}
