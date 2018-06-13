using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFTests.ApiRegression.Steps;
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
            string token = null;
            string country = null;
            string firstName = "Autotest";
            string lastName = "User";
            string clientInfo = "iPhone; Model:6 Plus; Os:9.3.5; Screen:414x736";
            string hint = "qwe";

            [Test]
          //  [Category("BlockchainIntegration")]
            public void ProhibotCashOutsToHWTest()
            {
                string phonePrefix = null;
                string phoneNumber = TestData.GenerateNumbers(9);

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

                #region register client

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

                //STEP 6
                var getPersonalData = walletApi.PersonalData
                    .GetPersonalDataResponse(token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                Assert.That(getPersonalData.GetResponseObject().Result
                        .Email, Is.EqualTo(email));

                //STEP 7
                var postClientFullName = walletApi.ClientFullName
                    .PostClientFullName(new PostClientFullNameModel() { FullName = $"{firstName} {lastName}" }, token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();

                //STEP 8
                getPersonalData = walletApi.PersonalData
                    .GetPersonalDataResponse(token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                var getPersonalDataResult = getPersonalData.GetResponseObject().Result;
                Assert.That(getPersonalDataResult.FullName, Is.EqualTo($"{firstName} {lastName}"));
                Assert.That(getPersonalDataResult.FirstName, Is.EqualTo(firstName));
                Assert.That(getPersonalDataResult.LastName, Is.EqualTo(lastName));
                Assert.That(getPersonalDataResult.Country, Is.Not.Null.And.Not.Empty);
                country = getPersonalData.GetResponseObject().Result.Country;

                //STEP 9
                var getCountryPhoneCodes = walletApi.CountryPhoneCodes
                    .GetCountryPhoneCodes(token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                var getCountryPhoneCodesResult = getCountryPhoneCodes.GetResponseObject().Result;
                Assert.That(getCountryPhoneCodesResult.Current, Is.EqualTo(country));
                phonePrefix = getCountryPhoneCodesResult.CountriesList
                    .FirstOrDefault(c => c.Id == country)?.Prefix;
                Assert.That(phonePrefix, Is.Not.Null);

                //STEP 10
                var postCheckMobilePhone = walletApi.CheckMobilePhone
                    .PostCheckMobilePhone(new PostClientPhoneModel() { PhoneNumber = phonePrefix + phoneNumber }, token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();

                //STEP 11
                var getCheckMobilePhone = walletApi.CheckMobilePhone
                    .GetCheckMobilePhone(phonePrefix + phoneNumber, code, token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                Assert.That(getCheckMobilePhone.GetResponseObject().Result
                    .Passed, Is.True);

                //STEP 12
                var getCheckDocumentsToUpload = walletApi.CheckDocumentsToUpload
                    .GetCheckDocumentsToUpload(token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                var getCheckDocumentsToUploadResult = getCheckDocumentsToUpload.GetResponseObject().Result;
                Assert.That(getCheckDocumentsToUploadResult.IdCard, Is.True);
                Assert.That(getCheckDocumentsToUploadResult.ProofOfAddress, Is.True);
                Assert.That(getCheckDocumentsToUploadResult.Selfie, Is.True);

                //STEP 13
                var postPinSecurity = walletApi.PinSecurity
                    .PostPinSecurity(new PinSecurityChangeModel() { Pin = pin }, token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();

                //STEP 14
                var getMyLykkeSettings = walletApi.MyLykkeSettings
                    .GetMyLykkeSettings(token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();
                Assert.That(getMyLykkeSettings.GetResponseObject().Result.MyLykkeEnabled,
                    Is.True);

                //STEP 15
                var postClientKeys = walletApi.ClientKeys
                    .PostClientKeys(new ClientKeysModel()
                    {
                        PubKey = bitcoinPrivateKey.PubKey.ToString(),
                        EncodedPrivateKey = AesUtils.Encrypt(bitcoinPrivateKey.ToString(), password)
                    }, token)
                    .Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError();

                #endregion

                var clientId = lykkePrivateApi.ClientAccount.ClientAccountInformation.
                    GetClientsByEmail(email).GetResponseObject().FirstOrDefault()?.Id;

                Assert.That(clientId, Is.Not.Null, "UnExpected ClientId is null.");

                var walletId = lykkePrivateApi.ClientAccount.Wallets.GetWalletsForClientById(clientId).
                    GetResponseObject().FirstOrDefault()?.Id;
                Assert.That(walletId, Is.Not.Null, $"Dont have any wallets for client {clientId}");

                // fill wallet with manual CashIn
                var manualCashIn = new ManualCashInRequestModel
                {
                    Amount = 10,
                    AssetId = currentAssetId,
                    ClientId = clientId,
                    Comment = "AutotestFund",
                    UserId = "Autotest user"
                };
                var cryptoToWalletResponse = lykkePrivateApi.ExchangeOperation.PostManualCashIn(manualCashIn);

                // we have crypto. Go to make CashOut
                var mobileSteps = new MobileSteps(walletApi);
                var keys = mobileSteps.Login(email, password, pin);
                var SignatureVerificationToken = mobileSteps.GetAccessToken(email, keys.token, keys.privateKey);
                
                //complete backup
                var backUp = walletApi.BackupCompleted.PostBackupCompleted(token);             

                var cashOut = new HotWalletCashoutOperation { AssetId = currentAssetId, DestinationAddress = currentBlockchainSettings.HotWalletAddress, Volume = 5 };

                var cashOutRequest = walletApi.HotWallet.PostCashOut(cashOut, SignatureVerificationToken, token);

                if (cashOutRequest.GetResponseObject().Error.Message.ToLower().Contains("address is invalid"))
                    Assert.Pass("Error message contain 'address is invalid'");
                else
                {
                    var getDiscl = walletApi.AssetDisclaimers.Get(token);
                    var postDiscl = walletApi.AssetDisclaimers.PostApproveById(getDiscl.GetResponseObject().Result.Disclaimers[0].Id, token);

                    //make cashout again
                    SignatureVerificationToken = mobileSteps.GetAccessToken(email, keys.token, keys.privateKey);
                    cashOutRequest = walletApi.HotWallet.PostCashOut(cashOut, SignatureVerificationToken, token);

                    Assert.That(cashOutRequest.GetResponseObject().Error.Message.ToLower(), Does.Contain("address is invalid"), "Unexpected error message");
                }
            }
        }
    }
}
