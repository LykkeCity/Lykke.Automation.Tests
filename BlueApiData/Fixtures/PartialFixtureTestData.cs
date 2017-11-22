using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BlueApiData.DTOs;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Entities.BlueApi;
using XUnitTestCommon.Tests;

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture : BaseTest
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
            TestClientId = this.TestPledgeClientIDs["GetPledge"];
            TestPledge = await CreateTestPledge(TestClientId, "GetPledge");
        }

        public async void PrepareCreateTestPledge()
        {
            TestPledgeCreateClientId = this.TestPledgeClientIDs["CreatePledge"];
            TestPledge = await CreateTestPledge(TestPledgeCreateClientId, "CreatePledge");
        }

        public async Task PrepareUpdateTestPledge()
        {
            TestPledgeUpdateClientId = this.TestPledgeClientIDs["UpdatePledge"];
            TestPledgeUpdate = await CreateTestPledge(TestPledgeUpdateClientId, "UpdatePledge");
        }

        public async Task PrepareDeleteTestPledge()
        {
            TestPledgeDeleteClientId = this.TestPledgeClientIDs["DeletePledge"];
            TestPledgeDelete = await CreateTestPledge(TestPledgeDeleteClientId, "DeletePledge");
        }

        public void PrepareTwitterData()
        {
            AccountEmail = _configBuilder.Config["TwitterAccountEmail"];

        }

        public void GetTestClientId()
        {
            this.TestClientId = _configBuilder.Config["AuthClientId"];
        }
    }
}
