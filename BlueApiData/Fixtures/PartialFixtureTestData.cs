using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BlueApiData.DTOs;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Entities.BlueApi;

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture
    {
        private void PrepareMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IPledgeEntity, PledgeDTO>();
                cfg.CreateMap<PledgeEntity, PledgeDTO>();
            });

            Mapper = config.CreateMapper();
        }

        private async Task PrepareTestData()
        {
            PrepareMapper();

            _pledgesToDelete = new Dictionary<string, string>();

            TestClientId = _configBuilder.Config["AuthClientId"];
            TestPledgeCreateClientId = _configBuilder.Config["AuthPledgeCreateClientId"];
            TestPledgeDeleteClientId = _configBuilder.Config["AuthPledgeDeleteClientId"];
            TestPledgeUpdateClientId = _configBuilder.Config["AuthPledgeUpdateClientId"];

            AccountEmail = _configBuilder.Config["TwitterAccountEmail"];

            TestPledge = await CreateTestPledge(clientId: TestClientId);
            TestPledgeUpdate = await CreateTestPledge(TestPledgeUpdateClientId, "UpdatePledge");
            TestPledgeDelete = await CreateTestPledge(TestPledgeDeleteClientId, "DeletePledge");
        }
    }
}
