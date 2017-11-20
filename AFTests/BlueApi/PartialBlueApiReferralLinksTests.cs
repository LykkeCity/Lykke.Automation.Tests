using System;
using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests
    {
        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestInvitationLinkForNonExistingClient")]
        public async Task RequestInvitationLinkForNonExistingClient()
        {
            var url = $"{ApiPaths.REFERRAL_LINKS_BASE_PATH}/request/invitationLink";
            var body = JsonUtils.SerializeObject(new RequestInvitationLinkRequestDto { SenderClientId = "123" });

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestInvitationLinkForExistingClient")]
        public async Task RequestInvitationLinkForExistingClient()
        {
            var url = $"{ApiPaths.REFERRAL_LINKS_BASE_PATH}/request/invitationLink";
            var body = JsonUtils.SerializeObject(new RequestInvitationLinkRequestDto { SenderClientId = _fixture.TestClientId });

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.Created);

            var parsedResponse = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(response.ResponseJson);
            var refLinkId = Guid.Empty;

            Assert.True(!String.IsNullOrEmpty(parsedResponse.RefLinkId));
            Assert.True(Guid.TryParse(parsedResponse.RefLinkId, out refLinkId));
            Assert.True(refLinkId != Guid.Empty);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForNonExistingClient")]
        public async Task RequestGiftCoinsLinkForNonExistingClient()
        {
            var url = $"{ApiPaths.REFERRAL_LINKS_BASE_PATH}/request/giftCoinslLink";
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { SenderClientId = "123" });

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForInvalidAsset")]
        public async Task RequestGiftCoinsLinkForInvalidAsset()
        {
            var url = $"{ApiPaths.REFERRAL_LINKS_BASE_PATH}/request/giftCoinslLink";
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123" });

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForInvalidAmount")]
        public async Task RequestGiftCoinsLinkForInvalidAmount()
        {
            var url = $"{ApiPaths.REFERRAL_LINKS_BASE_PATH}/request/giftCoinslLink";
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Amount = 123 });

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestGiftCoinsLinkForInvalidAllParams")]
        public async Task RequestGiftCoinsLinkForInvalidAllParams()
        {
            var url = $"{ApiPaths.REFERRAL_LINKS_BASE_PATH}/request/giftCoinslLink";
            var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123", Amount = 123, SenderClientId = "123"});

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

            Assert.True(response.Status == HttpStatusCode.BadRequest);
        }
    }
}