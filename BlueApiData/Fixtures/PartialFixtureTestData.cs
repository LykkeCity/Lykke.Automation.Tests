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
using XUnitTestCommon;
using RestSharp;
using System.Net;
using XUnitTestCommon.Utils;
using BlueApiData.DTOs.ReferralLinks;
using XUnitTestCommon.GlobalActions;

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

        public async Task PrepareGlobalTestData()
        {
            Task<List<ApiConsumer>> registerUsersTasks = RegisterNUsers(2);

            this.GlobalConsumer = new ApiConsumer(this._configBuilder);
            await this.GlobalConsumer.RegisterNewUser();

            var createLinkResponse = await this.GlobalConsumer.ExecuteRequest(ApiPaths.REFERRAL_LINKS_INVITATION_PATH, Helpers.EmptyDictionary, null, Method.POST);
            if(createLinkResponse.Status == HttpStatusCode.Created)
            {
                this.TestInvitationLink = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(createLinkResponse.ResponseJson);
            }

            List<ApiConsumer> registeredUsers = await registerUsersTasks;
            foreach (ApiConsumer consumer in registeredUsers)
            {
                var body = new InvitationLinkClaimDTO()
                {
                    //ReferalLinkId = this.TestInvitationLink.RefLinkId,
                    ReferalLinkUrl = this.TestInvitationLink.RefLinkUrl,
                    IsNewClient = true
                };
                var response = await consumer.ExecuteRequest($"{ApiPaths.REFERRAL_LINKS_INVITATION_PATH}/{TestInvitationLink.RefLinkId}/claim", Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.PUT);
            }
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
            this.InvitationLinkClaimersConsumers = await RegisterNUsers(8);
        }

        public void PrepareTwitterData()
        {
            AccountEmail = _configBuilder.Config["TwitterAccountEmail"];
            if (!Boolean.TryParse(_configBuilder.Config["TwitterAggessiveCheck"], out TwitterAggressiveCheck))
                TwitterAggressiveCheck = false;
        }
    }
}
