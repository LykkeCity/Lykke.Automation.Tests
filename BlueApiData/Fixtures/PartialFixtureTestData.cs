using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BlueApiData.DTOs;
using XUnitTestData.Domains.BlueApi;
using XUnitTestData.Entities.BlueApi;
using XUnitTestCommon.Tests;
using System.Linq;
using XUnitTestCommon.Consumers;

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
            await CreatePledgeClientAndApiConsumer("GetPledge");
            TestPledge = await CreateTestPledge("GetPledge");
        }

        public async Task PrepareCreateTestPledge()
        {
            await CreatePledgeClientAndApiConsumer("CreatePledge");
        }

        public async Task PrepareUpdateTestPledge()
        {
            await CreatePledgeClientAndApiConsumer("UpdatePledge");
            TestPledgeUpdate = await CreateTestPledge("UpdatePledge");
        }

        public async Task PrepareDeleteTestPledge()
        {
            await CreatePledgeClientAndApiConsumer("DeletePledge");
            TestPledgeDelete = await CreateTestPledge("DeletePledge");
        }

        public async Task PrepareRequestInvitationLink()
        {
            this.InvitationLinkRequestConsumer = (await RegisterNUsers(1)).FirstOrDefault();
        }

        public async Task PrepareClainInvitationLink()
        {
            this.InvitationLinkClaimersConsumers = await RegisterNUsers(7);
            //give tree coins to client who creates invitation link
            await MEConsumer.Client.UpdateBalanceAsync(Guid.NewGuid().ToString(), InvitationLinkClaimersConsumers[0].ClientInfo.Account.Id, "TREE", 100.0);
        }

        public void PrepareTwitterData()
        {
            AccountEmail = _configBuilder.Config["TwitterAccountEmail"];

        }
    }
}
