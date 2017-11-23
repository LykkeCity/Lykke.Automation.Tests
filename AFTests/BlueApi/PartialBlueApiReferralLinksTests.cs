using System;
using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using BlueApiData.DTOs.ReferralLinks;

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

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.Created);

            var parsedResponse = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(response.ResponseJson);
            var refLinkId = Guid.Empty;

            Assert.True(!String.IsNullOrEmpty(parsedResponse.RefLinkId));
            Assert.True(Guid.TryParse(parsedResponse.RefLinkId, out refLinkId));
            Assert.True(refLinkId != Guid.Empty);

            //Attempt to request second invitation link should fail
            response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status != HttpStatusCode.Created);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForNonExistingClient")]
        public async Task RequestGiftCoinsLinkForNonExistingClient()
        {
            var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { SenderClientId = "123" });

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForInvalidAsset")]
        public async Task RequestGiftCoinsLinkForInvalidAsset()
        {
            var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123" });

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForInvalidAmount")]
        public async Task RequestGiftCoinsLinkForInvalidAmount()
        {
            var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Amount = 123 });

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForInvalidAllParams")]
        public async Task RequestGiftCoinsLinkForInvalidAllParams()
        {
            var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123", Amount = 123, SenderClientId = "123"});

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Ignore("Invalid test")]
        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ClaimInvitationLink")]
        public async Task ClaimInvitationLink()
        {
            //this.PrepareReferralLinksData();
            var url = ApiPaths.REFERRAL_LINKS_CLAIM_LINK_PATH;

            //send request without data 
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);

            var body = new InvitationLinkClaimDTO()
            {
                RecipientClientId = GlobalConstants.AutoTest,
                ReferalLinkId = GlobalConstants.AutoTest,
                ReferalLinkUrl = GlobalConstants.AutoTest,
                IsNewClient = false
            };

            //send request with wrong data 
            response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);

            //var validLink = await this.GetFirstLink();
            //body = new InvitationLinkClaimDTO()
            //{
            //    RecipientClientId = "",
            //    ReferalLinkId = validLink.Id,
            //    ReferalLinkUrl = validLink.Url,
            //    IsNewClient = false
            //};

            //send request without a client id
            response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);

            body.RecipientClientId = this.TestClientId;
            
            //send request with already claimed client
            response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }
    }
}