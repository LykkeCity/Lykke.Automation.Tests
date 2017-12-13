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
using BlueApiData;
using System.Web;
using Lykke.Service.Balances.AutorestClient.Models;
using Lykke.Service.Balances.Client.ResponseModels;
using System.Collections.Generic;
using System.Linq;

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

            var url = ApiPaths.REFERRAL_LINKS_INVITATION_PATH;
            var response = await this.InvitationLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);

            Assert.True(response.Status == HttpStatusCode.Created);

            var parsedResponse = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(response.ResponseJson);
            var refLinkId = Guid.Empty;

            Assert.True(!String.IsNullOrEmpty(parsedResponse.RefLinkId));
            Assert.True(Guid.TryParse(parsedResponse.RefLinkId, out refLinkId));
            Assert.True(refLinkId != Guid.Empty);

            ReferralLinkEntity entity = await this.ReferralLinkRepository.TryGetAsync(parsedResponse.RefLinkId) as ReferralLinkEntity;
            Assert.True(entity.Type == "Invitation");
            Assert.True(entity.State == "Created");
            Assert.True(entity.Asset == Constants.TREE_COIN_ID);
            Assert.True(entity.Url == parsedResponse.RefLinkUrl);

            //Attempt to request second invitation link should fail
            response = await this.InvitationLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status != HttpStatusCode.Created);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ClaimInvitationLink")]
        public async Task ClaimInvitationLink()
        {
            await this.PrepareClainInvitationLink();

            var url = $"{ApiPaths.REFERRAL_LINKS_PATH}/{GlobalConstants.AutoTest}/claim";

            //send request without data 
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NotFound);

            var body = new InvitationLinkClaimDTO()
            {
                ReferalLinkUrl = GlobalConstants.AutoTest,
                IsNewClient = false
            };

            //send request with wrong data 
            response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NotFound);



            //Create link to be claimed
            var createLinkUrl = ApiPaths.REFERRAL_LINKS_INVITATION_PATH;
            ApiConsumer createLinkConsumer = this.InvitationLinkClaimersConsumers[0];
            this.InvitationLinkClaimersConsumers.RemoveAt(0);

            var createLinkResponse = await createLinkConsumer.ExecuteRequest(createLinkUrl, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(createLinkResponse.Status == HttpStatusCode.Created);
            var createdLink = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(createLinkResponse.ResponseJson);

            body = new InvitationLinkClaimDTO()
            {
                ReferalLinkUrl = createdLink.RefLinkUrl,
                IsNewClient = true
            };
            string claimParam = JsonUtils.SerializeObject(body);

            url = $"{ApiPaths.REFERRAL_LINKS_INVITATION_PATH}/{createdLink.RefLinkId}/claim";

            for (int i = 0; i < this.InvitationLinkClaimersConsumers.Count; i++)
            {
                ApiConsumer claimConsumer = this.InvitationLinkClaimersConsumers[i];
                var claimResponse = await claimConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, claimParam, Method.PUT);
                InvitationLinkClaimResponseDTO parsedClaimResponse = JsonUtils.DeserializeJson<InvitationLinkClaimResponseDTO>(claimResponse.ResponseJson);

                ClientBalanceResponseModel senderBalance = null;
                ClientBalanceResponseModel recieverBalance = null;

                if (Constants.TREE_COIN_INVIRATION_AWARD != 0.0)
                {
                    List<ClientBalanceResponseModel> senderBalances = (await this.BalancesClient.GetClientBalances(createLinkConsumer.ClientInfo.Account.Id)).ToList();
                    senderBalance = senderBalances.Where(b => b.AssetId == Constants.TREE_COIN_ID).FirstOrDefault();

                    List<ClientBalanceResponseModel> recieverBalances = (await this.BalancesClient.GetClientBalances(claimConsumer.ClientInfo.Account.Id)).ToList();
                    recieverBalance = recieverBalances.Where(b => b.AssetId == Constants.TREE_COIN_ID).FirstOrDefault();
                }

                var statisticsResponse = await createLinkConsumer.ExecuteRequest($"{ApiPaths.REFERRAL_LINKS_PATH}/statistics", Helpers.EmptyDictionary, null, Method.GET);
                RefLinksStatisticsDTO linkStatistics = JsonUtils.DeserializeJson<RefLinksStatisticsDTO>(statisticsResponse.ResponseJson);

                //assert first five claimers should claim successfully and recieve reward
                if (i < 6)
                {
                    //Assert.True(Guid.TryParse(parsedClaimResponse.TransactionRewardSender, out Guid temp1));
                    //Assert.True(Guid.TryParse(parsedClaimResponse.TransactionRewardRecipient, out Guid temp2));
                    if (Constants.TREE_COIN_INVIRATION_AWARD != 0.0)
                    {
                        Assert.True(senderBalance.Balance == (i + 1) * Constants.TREE_COIN_INVIRATION_AWARD);
                        Assert.True(recieverBalance.Balance == Constants.TREE_COIN_INVIRATION_AWARD);
                    }
                }
                else
                {
                    //Assert.Null(parsedClaimResponse.TransactionRewardSender);
                    //Assert.Null(parsedClaimResponse.TransactionRewardRecipient);
                    if (Constants.TREE_COIN_INVIRATION_AWARD != 0.0)
                    {
                        Assert.True(senderBalance.Balance == 5 * Constants.TREE_COIN_INVIRATION_AWARD);
                        Assert.Null(recieverBalance);
                    }
                }

                Assert.True(linkStatistics.NumberOfInvitationLinksAccepted == i + 1);

                //attempt to claim again with single user should result in error
                if (i == 0)
                {
                    var secondClaimResponse = await claimConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, claimParam, Method.POST);
                    Assert.True(secondClaimResponse.Status != HttpStatusCode.OK);
                }
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksGet")]
        public async Task GetRefLinkDataById()
        {
            string url = ApiPaths.REFERRAL_LINKS_PATH + "/" + this.TestInvitationLink.RefLinkId;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            RefferalLinkDataDTO parsedResponse = JsonUtils.DeserializeJson<RefferalLinkDataDTO>(response.ResponseJson);

            Assert.True(parsedResponse.Id == this.TestInvitationLink.RefLinkId);
            Assert.True(parsedResponse.Url == this.TestInvitationLink.RefLinkUrl);
            Assert.True(parsedResponse.Asset == Constants.TREE_COIN_ID);
            Assert.True(parsedResponse.State == "Created");
            Assert.True(parsedResponse.Amount == Constants.TREE_COIN_INVIRATION_AWARD);
            Assert.True(parsedResponse.Type == "Invitation");

            if (Constants.TREE_COIN_INVIRATION_AWARD != 0.0)
                Assert.True(parsedResponse.SenderClientId == this.GlobalConsumer.ClientInfo.Account.Id);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksGet")]
        public async Task GetRefLinkDataByUrl()
        {
            string url = ApiPaths.REFERRAL_LINKS_PATH + "/url/" + HttpUtility.UrlEncode(this.TestInvitationLink.RefLinkUrl);
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            RefferalLinkDataDTO parsedResponse = JsonUtils.DeserializeJson<RefferalLinkDataDTO>(response.ResponseJson);

            Assert.True(parsedResponse.Id == this.TestInvitationLink.RefLinkId);
            Assert.True(parsedResponse.Url == this.TestInvitationLink.RefLinkUrl);
            Assert.True(parsedResponse.Asset == Constants.TREE_COIN_ID);
            Assert.True(parsedResponse.State == "Created");
            Assert.True(parsedResponse.Amount == Constants.TREE_COIN_INVIRATION_AWARD);
            Assert.True(parsedResponse.Type == "Invitation");

            if (Constants.TREE_COIN_INVIRATION_AWARD != 0.0)
                Assert.True(parsedResponse.SenderClientId == this.GlobalConsumer.ClientInfo.Account.Id);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksGet")]
        public async Task GetRefLinkStatistics()
        {
            string url = ApiPaths.REFERRAL_LINKS_PATH + "/statistics";
            var response = await this.GlobalConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            RefLinksStatisticsDTO parsedResponse = JsonUtils.DeserializeJson<RefLinksStatisticsDTO>(response.ResponseJson);

            Assert.True(parsedResponse.NumberOfInvitationLinksSent == 1);
            Assert.True(parsedResponse.NumberOfInvitationLinksAccepted == 2);
        }
    }
}