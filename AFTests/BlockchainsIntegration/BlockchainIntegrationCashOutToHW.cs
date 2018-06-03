using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFTests.BlockchainsIntegrationTests;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.Api;
using LykkeAutomationPrivate.DataGenerators;
using Newtonsoft.Json;
using NUnit.Framework;
using XUnitTestCommon.ServiceSettings;
using XUnitTestCommon.TestsData;

namespace AFTests.BlockchainsIntegrationTests
{
    class BlockchainIntegrationCashOutToHW
    {

        internal class BlockchainSettings
        {
            public string Type { get; set; }
            public string ApiUrl { get; set; }
            public string SignServiceUrl { get; set; }
            public string HotWalletAddress { get; set; }
            public Monitoring Monitoring { get; set; }
        }

        public class Monitoring
        {
            public string InProgressOperationAlarmPeriod { get; set; }
        }

        public class ProhibotCashOutsToHW : BlockchainsIntegrationBaseTest
        {
            protected WalletApi.Api.WalletApi walletApi = new WalletApi.Api.WalletApi();
            protected LykkeApi lykkePrivateApi = new LykkeApi();

            string email = TestData.GenerateEmail();
            string password = Guid.NewGuid().ToString("N").Substring(0, 10);
            string pin = "1111";
            string code = "0000";

            [Test]
           // [Category("BlockchainIntegration")]
            public void ProhibotCashOutsToHWTest()
            {
                string firstName = "Autotest";
                string lastName = "User";
                string clientInfo = "iPhone; Model:6 Plus; Os:9.3.5; Screen:414x736";
                string hint = "qwe";

                string phonePrefix = null;
                string phoneNumber = TestData.GenerateNumbers(9);
                string token = null;
                string country = null;

                var currentAssetId = lykkePrivateApi.Assets.GetAssets().GetResponseObject().FirstOrDefault(a => 
                {
                    if (a.BlockchainIntegrationLayerId != null)
                        return a.BlockchainIntegrationLayerId.ToString().ToLower() == BlockChainName.ToLower();

                    return false;
                    })?.Id;

                var blockchainSettings = cfg["BlockchainsIntegration"];

                var currentBlockchainSettings = JsonConvert.DeserializeObject<BlockchainSettings[]>(cfg["BlockchainsIntegration"]["Blockchains"].ToString()).FirstOrDefault(b => b.Type.ToLower().Contains(BlockChainName.ToLower()));

                if (currentBlockchainSettings == null)
                    Assert.Ignore($"Blockchain {BlockChainName} does not present in blockchain settings {blockchainSettings}");

                var bitcoinPrivateKey = new NBitcoin.Key().GetWif(NBitcoin.Network.TestNet);

                //STEP 0
                var getApplicationInfo = walletApi.ApplicationInfo
                    .GetApplicationInfo()
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();

                //STEP 2
                var postEmailVerification = walletApi.EmailVerification
                    .PostEmailVerification(new PostEmailModel() { Email = email })
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                Assert.That(postEmailVerification.GetResponseObject().Error, Is.Null);

                //STEP 3
                var getEmailVerification = walletApi.EmailVerification
                    .GetEmailVerification(email, code, null)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                Assert.That(getEmailVerification.GetResponseObject().Result.Passed, Is.True);

                //STEP 4
                var postRegistration = walletApi.Registration.PostRegistrationResponse(new AccountRegistrationModel()
                {
                    ClientInfo = clientInfo,
                    Email = email,
                    Hint = hint,
                    Password = Sha256.GenerateHash(password)
                })
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
                Assert.Multiple(() =>
                {
                    var postRegistrationData = postRegistration.GetResponseObject();
                    Assert.That(postRegistrationData.Result.PersonalData?.Email, Is.EqualTo(email));
                    Assert.That(postRegistrationData.Result.Token, Is.Not.Null.And.Not.Empty);
                });
                token = postRegistration.GetResponseObject().Result.Token;

                var clientId = lykkePrivateApi.ClientAccount.ClientAccountInformation.
                    GetClientsByEmail(email).GetResponseObject().FirstOrDefault()?.Id;

                Assert.That(clientId, Is.Not.Null, "UnExpected ClientId is null.");

                var walletId = lykkePrivateApi.ClientAccount.Wallets.GetWalletsForClientById(clientId).
                    GetResponseObject().FirstOrDefault()?.Id;
                Assert.That(walletId, Is.Not.Null, $"Dont have any wallets for client {clientId}");

                // fill wallet with manual CashIn
                var manualCashIn = new ManualCashInRequestModel { Amount = 10, AssetId = currentAssetId, ClientId = clientId, Comment = "AutotestFund", UserId = "Autotest user" };
                var cryptoToWalletResponse = lykkePrivateApi.ExchangeOperation.PostManualCashIn(manualCashIn);

                // we have crypto. Go to make CashOut

                var signTpken = walletApi.SignatureVerificationToken.GetKeyConfirmation(email, token).GetResponseObject().Result.Message;

                var cashOut = new HotWalletCashoutOperation { AssetId = currentAssetId, DestinationAddress = currentBlockchainSettings.HotWalletAddress, Volume = 5 };

                var cashOutRequest = walletApi.HotWallet.PostCashOut(cashOut, signTpken, token);

                //Add validation here
            }
        }
    }
}
