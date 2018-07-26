using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.DataGenerators;
using NUnit.Framework;
using XUnitTestCommon.TestsData;

namespace AFTests.ApiRegression
{
    public class RegisterTests : ApiRegressionBaseTest
    {
        string email = TestData.GenerateEmail();
        string password = Guid.NewGuid().ToString("N").Substring(0, 10);
        string pin = "1111";
        string code = "0000";

        [Test]
        [Order(1)]
        [Category("ApiRegression")]
        public void RegisterTest()
        {
            string firstName = "Autotest";
            string lastName = "User";
            string clientInfo = "iPhone; Model:6 Plus; Os:9.3.5; Screen:414x736";
            string hint = "qwe";
            
            string phonePrefix = null;
            string phoneNumber = TestData.GenerateNumbers(9);
            string token = null;
            string country = null;

            var bitcoinPrivateKey = new NBitcoin.Key().GetWif(NBitcoin.Network.TestNet);

            //STEP 0
            var getApplicationInfo = walletApi.ApplicationInfo
                .GetApplicationInfo()
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();

            //STEP 1
            ////var getClientState = walletApi.ClientState
            ////    .GetClientState(email, null)
            ////    .Validate.StatusCode(HttpStatusCode.OK)
            ////    .Validate.NoApiError();
            ////Assert.That(getClientState.GetResponseObject().Result
            ////        .IsRegistered, Is.False);

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

            ////STEP 5
            //getClientState = walletApi.ClientState
            //    .GetClientState(email, null)
            //    .Validate.StatusCode(HttpStatusCode.OK)
            //    .Validate.NoApiError();
            //Assert.Multiple(() =>
            //{
            //    var getClientStateData = getClientState.GetResponseObject();
            //    Assert.That(getClientStateData.Result.IsRegistered, Is.True);
            //    Assert.That(getClientStateData.Result.IsPwdHashed, Is.True);
            //});

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
        }

        [Test]
        [Order(2)]
        [Category("ApiRegression")]
        public void LoginTest()
        {
            string clientInfo = "<android>; Model:<Android SDK built for x86>; Os:<android>; Screen:<1080x1794>;";
            string code = "0000";
            string token = null;
            string accessToken = null;
            string encodedPrivateKey = null;

            ApplicationInfoResponseModel appInfo = null;

            //STEP 1
            var getClientState = walletApi.ClientState
                .GetClientState(email, null)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(getClientState.GetResponseObject().Result
                .IsRegistered, Is.True, "Trying to login by unregistered account");

            //STEP 2
            var postAuth = walletApi.Auth
                .PostAuthResponse(new AuthenticateModel()
                {
                    ClientInfo = clientInfo,
                    Email = email,
                    Password = Sha256.GenerateHash(password)
                })
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            var postAuthResult = postAuth.GetResponseObject().Result;
            Assert.That(postAuthResult.PersonalData.Email, Is.EqualTo(email));
            Assert.That(postAuthResult.Token, Is.Not.Null);
            token = postAuthResult.Token;

            //STEP 3
            var getCheckDocumentsToUpload = walletApi.CheckDocumentsToUpload
                .GetCheckDocumentsToUpload(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();

            //STEP 4
            var getPinSecurity = walletApi.PinSecurity
                .GetPinSecurity(pin, token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(getPinSecurity.GetResponseObject().Result
                .Passed, Is.True);

            //STEP 5
            var getClientCode = walletApi.Client
                .GetClientCodes(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();

            //STEP 6
            var postClientCodes = walletApi.Client
                .PostClientCodes(new SubmitCodeModel()
                {
                    Code = code
                }, token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            accessToken = postClientCodes.GetResponseObject().Result.AccessToken;
            Assert.That(accessToken, Is.Not.Null);

            //STEP 7 POST https://api-test.lykkex.net/api/Client/keys/encodedmainkey 
            var postEncodedMainKey = walletApi.Client
                .PostClientEncodedMainKey(new AccessTokenModel
                {
                    AccessToken = accessToken
                }, token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            encodedPrivateKey = postEncodedMainKey.GetResponseObject().Result.EncodedPrivateKey;
            Assert.That(encodedPrivateKey, Is.Not.Null);

            //STEP 8
            getClientState = walletApi.ClientState
                .GetClientState(email, null)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();

            //STEP 9 GET https://api-test.lykkex.net/api/Dicts/Assets
            var getDictAssets = walletApi.Dicts.GetAssetts(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 10 GET https://api-test.lykkex.net/api/BaseAssets
            var getBaseAssets = walletApi.BaseAssets.GetBaseAssets(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 11 GET https://api-test.lykkex.net/api/AppSettings
            var getAppSettings = walletApi.AppSettings.GetAppSettings(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 12 GET https://api-test.lykkex.net/api/ApplicationInfo
            var getApplicationInfo = walletApi.ApplicationInfo.GetApplicationInfo()
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            appInfo = getApplicationInfo.GetResponseObject().Result;
            //TODO: Add asserts

            //STEP 13 GET https://api-test.lykkex.net/api/AssetPairs
            var getAssetPairs = walletApi.AssetPairs.GetAssetPairs(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 14 GET https://api-test.lykkex.net/api/Issuers
            var getIssuers = walletApi.Issuers.Get(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 15 GET https://api-test.lykkex.net/api/Client/pendingActions
            var getPendingActions = walletApi.Client.GetClientPendingActions(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 16 GET https://api-test.lykkex.net/api/offchain/limit/list
            var getOffchainLimitList = walletApi.LimitOrders.GetOffchainLimitList(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 17 GET https://api-test.lykkex.net/api/offchain/limit/count
            var getOffchainLimitCount = walletApi.LimitOrders.GetOffchainLimitCount(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 18 GET https://api-test.lykkex.net/api/Dictionary/ico
            var getDictionaryIco = walletApi.Dictionary.GetDictionaryKey("ico")
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 19 GET https://api-test.lykkex.net/api/assetcategories
            var getAssetCategories = walletApi.AssetsCategories.GetAssetsCategories(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 20 GET https://mt-api-test.lykkex.net/api/init/data
            //TODO: ADD

            //STEP 21 GET https://api-test.lykkex.net/api/Wallets
            var getWallets = walletApi.Wallets.GetWalltes(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 22 GET https://mt-api-test.lykkex.net/api/watchlists
            //var getWatchlists = walletApi.WatchLists.Get(token)
            //    .Validate.StatusCode(HttpStatusCode.OK)
            //    .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 23 GET https://mt-api-test.lykkex.net/api/orders
            //TODO: ADD

            //STEP 24 GET https://api-test.lykkex.net/api/offchain/limit/count
            getOffchainLimitCount = walletApi.LimitOrders.GetOffchainLimitCount(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts

            //STEP 25 GET https://api-test.lykkex.net/api/offchain/limit/list
            getOffchainLimitList = walletApi.LimitOrders.GetOffchainLimitList(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            //TODO: Add asserts
        }
    }
}
