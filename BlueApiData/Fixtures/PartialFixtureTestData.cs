using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture
    {
        private async Task PrepareTestData()
        {
            ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["IsAlive"] = "/api/isAlive";
            ApiEndpointNames["Pledges"] = "/api/pledges";

            _pledgesToDelete = new Dictionary<string, string>();

            TestClientId = _configBuilder.Config["AuthClientId"];
            TestPledgeCreateClientId = _configBuilder.Config["AuthPledgeCreateClientId"];
            TestPledgeDeleteClientId = _configBuilder.Config["AuthPledgeDeleteClientId"];
            TestPledgeUpdateClientId = _configBuilder.Config["AuthPledgeUpdateClientId"];

            TestPledge = await CreateTestPledge(clientId: TestClientId);
            TestPledgeUpdate = await CreateTestPledge(TestPledgeUpdateClientId, "UpdatePledge");
            TestPledgeDelete = await CreateTestPledge(TestPledgeDeleteClientId, "DeletePledge");
        }
    }
}
