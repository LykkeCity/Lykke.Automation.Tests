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

        public async Task PrepareDefaultTestPledge()
        {
            TestClientId = _configBuilder.Config["AuthClientId"];
            TestPledge = await CreateTestPledge(clientId: TestClientId);
        }

        public void PrepareCreateTestPledge()
        {
            TestPledgeCreateClientId = _configBuilder.Config["AuthPledgeCreateClientId"];
        }

        public async Task PrepareUpdateTestPledge()
        {
            TestPledgeUpdateClientId = _configBuilder.Config["AuthPledgeUpdateClientId"];
            TestPledgeUpdate = await CreateTestPledge(TestPledgeUpdateClientId, "UpdatePledge");
        }

        public async Task PrepareDeleteTestPledge()
        {
            TestPledgeDeleteClientId = _configBuilder.Config["AuthPledgeDeleteClientId"];
            TestPledgeDelete = await CreateTestPledge(TestPledgeDeleteClientId, "DeletePledge");
        }

        public void PrepareTwitterData()
        {
            AccountEmail = _configBuilder.Config["TwitterAccountEmail"];
            TwitterSearchQuery = "#dog"; // hard-coded for now, will think of a way to change it dynamically 
            TwitterSearchUntilDate = DateTime.Parse("07-11-2017"); // hard-coded for now, will think of a way to change it dynamically
        }
    }
}
