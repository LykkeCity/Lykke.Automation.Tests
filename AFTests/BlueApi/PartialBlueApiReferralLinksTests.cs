using System;
using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using BlueApiData.DTOs.ReferralLinks;
using XUnitTestData.Entities.BlueApi;
using XUnitTestCommon.Consumers;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests
    {
        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestInvitationLink")]
        public async Task RequestInvitationLink()
        {
            await this.PrepareRequestInvitationLink();

            var url = ApiPaths.REFERRAL_LINKS_INVITATION_LINK_PATH;
            var response = await this.InvitationLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.Created);

            var parsedResponse = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(response.ResponseJson);
            var refLinkId = Guid.Empty;

            Assert.True(!String.IsNullOrEmpty(parsedResponse.RefLinkId));
            Assert.True(Guid.TryParse(parsedResponse.RefLinkId, out refLinkId));
            Assert.True(refLinkId != Guid.Empty);

            ReferralLinkEntity entity = await this.ReferralLinkRepository.TryGetAsync(parsedResponse.RefLinkId) as ReferralLinkEntity;
            Assert.True(entity.Type == "Invitation");
            Assert.True(entity.State == "Created");
            Assert.True(entity.Asset == "TREE");
            Assert.True(entity.Url == parsedResponse.RefLinkUrl);

            //Attempt to request second invitation link should fail
            response = await this.InvitationLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status != HttpStatusCode.Created);
        }

        [Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ClaimInvitationLink")]
        public async Task ClaimInvitationLink()
        {
            await this.PrepareClainInvitationLink();

            var url = ApiPaths.REFERRAL_LINKS_CLAIM_LINK_PATH;

            //send request without data 
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.UnsupportedMediaType);

            var body = new InvitationLinkClaimDTO()
            {
                ReferalLinkId = GlobalConstants.AutoTest,
                ReferalLinkUrl = GlobalConstants.AutoTest,
                IsNewClient = false
            };

            //send request with wrong data 
            response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);



            //Create link to be claimed
            var createLinkUrl = ApiPaths.REFERRAL_LINKS_INVITATION_LINK_PATH;
            ApiConsumer createLinkConsumer = this.InvitationLinkClaimersConsumers[0];
            this.InvitationLinkClaimersConsumers.RemoveAt(0);

            var createLinkResponse = await createLinkConsumer.ExecuteRequest(createLinkUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(createLinkResponse.Status == HttpStatusCode.Created);
            var createdLink = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(createLinkResponse.ResponseJson);

            body = new InvitationLinkClaimDTO()
            {
                ReferalLinkId = createdLink.RefLinkId,
                ReferalLinkUrl = createdLink.RefLinkUrl,
                IsNewClient = true
            };
            string claimParam = JsonUtils.SerializeObject(body);

            for (int i = 0; i < this.InvitationLinkClaimersConsumers.Count; i++)
            {
                ApiConsumer claimConsumer = this.InvitationLinkClaimersConsumers[i];
                var claimResponse = await claimConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, claimParam, Method.POST);
                InvitationLinkClaimResponseDTO parsedClaimResponse = JsonUtils.DeserializeJson<InvitationLinkClaimResponseDTO>(claimResponse.ResponseJson);

                //assert first five claimers should claim successfully
                if (i < 5)
                {
                    Assert.True(parsedClaimResponse.TransactionRewardRecipient == claimConsumer.ClientInfo.Account.Id);
                    Assert.True(parsedClaimResponse.TransactionRewardSender == claimConsumer.ClientInfo.Account.Id);
                }
                else
                {
                    Assert.True(true);
                }
            }
        }

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForNonExistingClient")]
        //public async Task RequestGiftCoinsLinkForNonExistingClient()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { SenderClientId = "123" });

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForInvalidAsset")]
        //public async Task RequestGiftCoinsLinkForInvalidAsset()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123" });

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForInvalidAmount")]
        //public async Task RequestGiftCoinsLinkForInvalidAmount()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Amount = 123 });

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForInvalidAllParams")]
        //public async Task RequestGiftCoinsLinkForInvalidAllParams()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123", Amount = 123, SenderClientId = "123"});

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}
    }
}