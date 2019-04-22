using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.DataGenerators;
using NBitcoin;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using XUnitTestCommon.TestsData;

namespace AFTests.ApiV2
{
    class E2ETests
    {
        public class E2EBaseTest : ApiV2BaseTest
        {
            #region setup teardown

            protected string BearerToken = "";
            readonly string lykkeWalletTokenURL = "https://ironclad-decorator-test.lykkex.net/getlykkewallettoken";
            protected readonly string pin = "1111";

            public string GetBearerToken()
            {
                string _once = TestData.GenerateString(20);
                string _state = TestData.GenerateString(20); ;
                var _guid = Guid.NewGuid().ToString();

                //1 GET ironclad decorator (Cookie)
                var ironClad = ironCladApi.GetIroncladAuthorize(_once, _state);
                Assert.That(ironClad.StatusCode, Is.EqualTo(HttpStatusCode.Found));
                var RedirectURL = ironClad.Headers.ToList().Find(h => h.Name == "Location").Value;
                var setCookie = ironClad.Cookies.First().ToString().Replace("Set-Cookie=", ""); ;
                //1

                //2 GET connect/authorize 
                var connect = lykkeTechAPI.GetConnect(RedirectURL.ToString(), setCookie);
                Assert.That(connect.StatusCode, Is.EqualTo(HttpStatusCode.Found));
                RedirectURL = connect.Headers.ToList().Find(h => h.Name == "Location").Value;
                //2

                //3 GET signInReturn(cookie)
                var recTockenRequest = lykkeTechAPI.GetConnect(RedirectURL.ToString(), setCookie);
                Assert.That(recTockenRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                var reg = new Regex("value=.{100,170}\"");
                var token = reg.Match(recTockenRequest.Content).Value.Replace("value=", "").Replace("\\", "").Replace("\"", "");
                var reqCookies = recTockenRequest.Cookies.First().ToString();
                var cookieRegExp = new Regex(".AspNetCore.Antiforgery.{50,170};");
                var postSignInCookie = cookieRegExp.Match(reqCookies).Value.ToString().Replace(";", "");
                //3

                //4 POST signInReturn  (with cookie 3)
                var postSignIn = lykkeTechAPI.PostSignIn(RedirectURL.ToString(), postSignInCookie, token, wallet.WalletAddress, wallet.WalletKey);

                Assert.That(postSignIn.StatusCode, Is.EqualTo(HttpStatusCode.Found));
                RedirectURL = postSignIn.Headers.ToList().Find(h => h.Name == "Location").Value;
                var cookiesForConnectAuthorize = "";
                var findedCookies = postSignIn.Cookies;
                findedCookies.ForEach(c => cookiesForConnectAuthorize += c + ";");

                //POST
                cookiesForConnectAuthorize = cookiesForConnectAuthorize.Replace("Set-Cookie=", "");

                //5, GET connect/autorize url (cookie from 4)
                var lykkeTechConnectAuthorize = lykkeTechAPI.GetConnect($"https://lykke.tech{RedirectURL}", cookiesForConnectAuthorize);
                Assert.That(lykkeTechConnectAuthorize.StatusCode, Is.EqualTo(HttpStatusCode.Found));

                var ironcladRedirectURL = lykkeTechConnectAuthorize.Headers.ToList().Find(h => h.Name == "Location").Value.ToString();

                //6 POST ironcladdecorator cookie 1, url 5

                var finish = lykkeTechAPI.GetConnect(ironcladRedirectURL, setCookie);

                Assert.That(finish.StatusCode, Is.EqualTo(HttpStatusCode.Found));
                var finishURL = finish.Headers.ToList().Find(h => h.Name == "Location").Value.ToString();

                token = finishURL.Split('&').ToList().Find(part => part.Contains("access_token")).Replace("access_token=", "");
                //3

                var getLykkeWalletTokenRequest = lykkeTechAPI.GetConnect(lykkeWalletTokenURL, setCookie, token);
                Assert.That(getLykkeWalletTokenRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                return JsonConvert.DeserializeObject<LykkeWalletTokenModel>(getLykkeWalletTokenRequest.Content).token;
            }

            [OneTimeSetUp]
            public void CashOutSetUp()
            {
                BearerToken = GetBearerToken();
            }
            #endregion
        }

        #region Models

        public class HistoryResponses
        {
            public HistoryResponse[] Responses { get; set; }
        }

        public class HistoryResponse
        {
            public string Type { get; set; }
            public float Volume { get; set; }
            public string AssetId { get; set; }
            public object BlockchainHash { get; set; }
            public string State { get; set; }
            public object FeeSize { get; set; }
            public string Id { get; set; }
            public string WalletId { get; set; }
            public DateTime Timestamp { get; set; }
        }

        /// <summary>
        /// 
        /// /
        /// </summary>
        /// 
        public class EtherBalanceObjectModel
        {
            public string status { get; set; }
            public string message { get; set; }
            public string result { get; set; }
        }


        public class LykkeWalletTokenModel
        {
            public string token { get; set; }
            public string authId { get; set; }
            public string notificationsId { get; set; }
        }

        #endregion


        public class E2EFist : E2EBaseTest
        {
            [Test]
            [Category("E2ESample")]
            public void E2ESampleTest()
            {

            }

            public bool WaitForLatestHistoryResponseGotFinishedState(string walletId, TimeSpan waitTime)
            {
                var url = $"http://history.services.svc.cluster.local/api/history?walletId={walletId}&offset=0&limit=1";
                var request = apiV2.CustomRequests.GetResponse(url);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed < waitTime)
                {
                    request = apiV2.CustomRequests.GetResponse(url);
                    if (request.StatusCode == HttpStatusCode.InternalServerError)
                        throw new Exception("History service is down");

                    if (JsonConvert.DeserializeObject<HistoryResponse[]>(request.Content).First().State == "Finished")
                    {
                        sw.Stop();
                        return true;
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                }
                sw.Stop();
                return false;
            }

            public string GetLatestFinishedTransactionId(string walletId)
            {
                var url = $"http://history.services.svc.cluster.local/api/history?walletId={walletId}&offset=0&limit=1";
                var request = apiV2.CustomRequests.GetResponse(url);

                request = apiV2.CustomRequests.GetResponse(url);
                if (request.StatusCode == HttpStatusCode.InternalServerError)
                    throw new Exception("History service is down");

                return JsonConvert.DeserializeObject<HistoryResponse[]>(request.Content).First().Id;

            }

            public string GetApi1SignVerToken()
            {
                var signTokenResponse = WalletApi.SignatureVerificationToken.GetKeyConfirmation(wallet.WalletAddress, BearerToken);
                Assert.That(signTokenResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var signToken = signTokenResponse.ResponseObject.Result.Message.ToString();
                var signVerToken = signToken;

                //
                WalletApi.PinSecurity.GetPinSecurity(pin, BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError("Please Reset Pin for user in Back Office");

                WalletApi.Client.GetClientCodes(BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

                string accessToken = WalletApi.Client.PostClientCodes(new SubmitCodeModel("0000"), BearerToken).Validate.StatusCode(HttpStatusCode.OK)
                    .Validate.NoApiError().GetResponseObject().Result.AccessToken;

                string encodedPrivateKey = WalletApi.Client
                    .PostClientEncodedMainKey(new AccessTokenModel(accessToken), BearerToken)
                    .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                    .GetResponseObject().Result.EncodedPrivateKey;

                string privateKeyStr = AesUtils.Decrypt(encodedPrivateKey, wallet.WalletKey);
                Key privateKey = Key.Parse(privateKeyStr);
                //

                signToken = privateKey.SignMessage(signToken);

                var postSignResponse = WalletApi.SignatureVerificationToken.PostKeyConfirmation(new RecoveryTokenChallangeResponse { Email = wallet.WalletAddress, SignedMessage = signToken }, BearerToken);
                Assert.That(postSignResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                return postSignResponse.ResponseObject.Result.AccessToken;
            }

            //ready
            [Test]
            [Category("ApiV2")]
            [Category("E2E")]
            [Category("ApiV2Operations")]
            public void E2EOrdinaryFlowApi1Test()
            {
                string initialEtherBalance = "";
                string finalEtherBalance = "";
                string DESTNATION_ADDRESS = "0x856924997fa22efad8dc75e83acfa916490989a4";
                var EtherBalanceAPIURL = $"https://api-ropsten.etherscan.io/api?module=account&action=balance&address={DESTNATION_ADDRESS}&tag=latest";
                var walletBalance = 0d;
                var finalBalance = 0d;
                var assetId = "e58aa37d-dd46-4bdb-bac1-a6d0d44e6dc9";
                var signToken = "";
                var signVerToken = "";
                var pin = "1111";
                var signedVerificationToken = "";
                var WalletId = "da268259-c2f1-414a-96b1-e0fd5b9b724f";
                var zeroTransactionId = "";
                var volume = 0.001;

                Step($"Get latest transaction id for wallet {WalletId}", () =>
                {
                    zeroTransactionId = GetLatestFinishedTransactionId(WalletId);
                });

                Step("Get initial balance of Ether waller: bcn initial balance = GET https://ropsten.etherscan.io/address/0x856924997fa22efad8dc75e83acfa916490989a4   Balance: % somebalance % Ether-> % inicialBcnBalance % ", () =>
                {
                    var balancePage = apiV2.CustomRequests.GetResponse(EtherBalanceAPIURL);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial ETHER balance");

                    initialEtherBalance = JsonConvert.DeserializeObject<EtherBalanceObjectModel>(balancePage.Content).result;
                });

                Step("Get initial balance of lykke client wallet. initial balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb - bac1 - a6d0d44e6dc9 assets.Balance-> % initialBalance % ", () =>
                {
                    var wallet = WalletApi.Wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    walletBalance = assetBalance.Balance;
                });

                Step("GET https://api-test.lykkex.net/api/signatureVerificationToken/KeyConfirmation?email=%email%", () =>
                {
                    var signTokenResponse = WalletApi.SignatureVerificationToken.GetKeyConfirmation(wallet.WalletAddress, BearerToken);
                    Assert.That(signTokenResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                    signToken = signTokenResponse.ResponseObject.Result.Message.ToString();
                    signVerToken = signToken;

                    //
                    //WalletApi.PinSecurity.GetPinSecurity(pin, BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError("Please Reset Pin for user in Back Office");

                    //WalletApi.PinSecurity.PostCheckPinSecurity(new PinSecurityCheckRequestModel {Pin = pin }, BearerToken);

                    WalletApi.Client.GetClientCodes(BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

                    string accessToken = WalletApi.Client.PostClientCodes(new SubmitCodeModel("0000"), BearerToken).Validate.StatusCode(HttpStatusCode.OK)
                        .Validate.NoApiError().GetResponseObject().Result.AccessToken;

                    string encodedPrivateKey = WalletApi.Client
                        .PostClientEncodedMainKey(new AccessTokenModel(accessToken), BearerToken)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result.EncodedPrivateKey;

                    string privateKeyStr = AesUtils.Decrypt(encodedPrivateKey, wallet.WalletKey);
                    Key privateKey = Key.Parse(privateKeyStr);
                    //

                    signToken = privateKey.SignMessage(signToken);
                });

                Step("POST https://api-test.lykkex.net/api/signatureVerificationToken/KeyConfirmation", () =>
                {
                    var postSignResponse = WalletApi.SignatureVerificationToken.PostKeyConfirmation(new RecoveryTokenChallangeResponse { Email = wallet.WalletAddress, SignedMessage = signToken }, BearerToken);
                    Assert.That(postSignResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    signedVerificationToken = postSignResponse.ResponseObject.Result.AccessToken;
                });

                Step("POST https://api-test.lykkex.net/api/HotWallet/cashout", () =>
                {
                    var postCashOut = WalletApi.HotWallet.PostCashOut(new HotWalletCashoutOperation
                    {
                        AssetId = assetId,
                        DestinationAddress = DESTNATION_ADDRESS,
                        DestinationAddressExtension = "",
                        Volume = volume
                    }, signedVerificationToken, BearerToken);

                    Assert.That(postCashOut.StatusCode, Does.Not.EqualTo(HttpStatusCode.NotFound).And.Not.EqualTo(HttpStatusCode.InternalServerError).And.Not.EqualTo(HttpStatusCode.BadGateway), "Wrong Cashout Status code");

                });

                Step("Delay ~ 20min cashout confirmation and waiting fo Finished status", () =>
                {
                    if (!WaitForLatestHistoryResponseGotFinishedState(WalletId, TimeSpan.FromMinutes(20)))
                        Assert.Fail("Cashout comfirmation does not exist afer 20 minutes");
                    Assert.That(GetLatestFinishedTransactionId(WalletId), Does.Not.EqualTo(zeroTransactionId), "Latest transaction ID is the same as before test. History service does not know about our transaction. POST cashout failed?");
                });

                Step("Get Final Balance; final balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb  - bac1 - a6d0d44e6dc9 assets.Balance-> % finalBalance %      initialBalance + finalBalance = %Volume - %feeSize", () =>
                {
                    var wallet = WalletApi.Wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    finalBalance = assetBalance.Balance;
                });

                Step("bcn final balance = GET https://ropsten.etherscan.io/address/0x856924997fa22efad8dc75e83acfa916490989a4 . Validate that final balance on ether wallet is initial balance + volume cashout ", () =>
                {
                    var balancePage = apiV2.CustomRequests.GetResponse(EtherBalanceAPIURL);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial ETHER balance");

                    finalEtherBalance = JsonConvert.DeserializeObject<EtherBalanceObjectModel>(balancePage.Content).result;

                    Assert.That(() => decimal.Parse(JsonConvert.DeserializeObject<EtherBalanceObjectModel>(apiV2.CustomRequests.GetResponse(EtherBalanceAPIURL).Content).result), Is.EqualTo(decimal.Parse(initialEtherBalance) + (decimal)volume*(decimal)Math.Pow(10,18)).After(15).Minutes.PollEvery(2).Seconds);
                });

                Step("Get https://ropsten.etherscan.io/tx/%BlockchainHash% Checking the transaction in bcn explorer. Status Status=Success To = 0x856924997fa22efad8dc75e83acfa916490989a4 Value = 0.0001 Ether", () =>
                {

                });

                Step("POST https://backoffice-test.lykkex.net/Mock/MailMock/Find Phrase: cashoutbil@autotest.com BO->Development->mail mocks", () => { });

                Step("POST https://backoffice-test.lykkex.net/Mock/MailMock/GetEmailBody Email: cashoutbil@autotest.com Id: % letterId % Take the newest letter", () => { });

                Step("Cash out complete You have successfully performed cash out of 0.0001 ETH!", () => { });
            }

            //[Test]
            [Category("ApiV2")]
            [Category("E2E")]
            [Category("ApiV2Operations")]
            public void E2EOrdinaryFlowApi2Test()
            {
                string initialEtherBalance = "";
                string finalEtherBalance = "";
                string DESTNATION_ADDRESS = "0x856924997fa22efad8dc75e83acfa916490989a4";
                var EtherBalanceAPIURL = $"https://api-ropsten.etherscan.io/api?module=account&action=balance&address={DESTNATION_ADDRESS}&tag=latest";
                var walletBalance = 0d;
                var finalBalance = 0d;
                var assetId = "e58aa37d-dd46-4bdb-bac1-a6d0d44e6dc9";
                var WalletId = "da268259-c2f1-414a-96b1-e0fd5b9b724f";

                var zeroTransactionId = "";

                Step($"Get latest transaction id for wallet {WalletId}", () =>
               {
                   zeroTransactionId = GetLatestFinishedTransactionId(WalletId);
               });

                Step("Get initial balance of Ether waller: bcn initial balance = GET https://ropsten.etherscan.io/address/0x856924997fa22efad8dc75e83acfa916490989a4   Balance: % somebalance % Ether-> % inicialBcnBalance % ", () =>
                {
                    var balancePage = apiV2.CustomRequests.GetResponse(EtherBalanceAPIURL);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial ETHER balance");

                    initialEtherBalance = JsonConvert.DeserializeObject<EtherBalanceObjectModel>(balancePage.Content).result;
                });

                Step("Get initial balance of lykke client wallet. initial balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb - bac1 - a6d0d44e6dc9 assets.Balance-> % initialBalance % ", () =>
                {
                    var wallet = apiV2.wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    walletBalance = assetBalance.Balance;
                });

                Step("POST https://apiv2-test.lykkex.net/api/operations/cashout/crypto", () =>
                {
                    var id = Guid.NewGuid().ToString();

                    var pCashOut = apiV2.Operations.PostOperationCashOut(new Lykke.Client.ApiV2.Models.CreateCashoutRequest
                    {
                        AssetId = assetId,
                        DestinationAddress = DESTNATION_ADDRESS,
                        DestinationAddressExtension = "",
                        Volume = 0.0001
                    }, "", BearerToken);

                    Assert.That(pCashOut.StatusCode, Does.Not.EqualTo(HttpStatusCode.NotFound).And.Not.EqualTo(HttpStatusCode.InternalServerError).And.Not.EqualTo(HttpStatusCode.BadGateway), "Wrong Cashout Status code");
                });

                Step("Delay ~ 20min cashout confirmation and waiting fo Finished status", () =>
                {
                    if (!WaitForLatestHistoryResponseGotFinishedState(WalletId, TimeSpan.FromMinutes(20)))
                        Assert.Fail("Cashout comfirmation does not exist afer 20 minutes");
                    Assert.That(GetLatestFinishedTransactionId(WalletId), Does.Not.EqualTo(zeroTransactionId), "Latest transaction ID is the same as before test. History service does not know about our transaction. POST cashout failed?");
                });

                Step("Get Final Balance; final balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb  - bac1 - a6d0d44e6dc9 assets.Balance-> % finalBalance %      initialBalance + finalBalance = %Volume - %feeSize", () =>
                {
                    var wallet = WalletApi.Wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    finalBalance = assetBalance.Balance;
                });

                Step("bcn final balance = GET https://ropsten.etherscan.io/address/0x856924997fa22efad8dc75e83acfa916490989a4 Balance: % somebalance % Ether-> % finalBcnBalance %   initialBalance - finalBalance = %Volume ", () =>
                {
                    var balancePage = apiV2.CustomRequests.GetResponse(EtherBalanceAPIURL);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial ETHER balance");

                    finalEtherBalance = JsonConvert.DeserializeObject<EtherBalanceObjectModel>(balancePage.Content).result;

                    Assert.That(long.Parse(finalEtherBalance), Is.EqualTo(long.Parse(initialEtherBalance) + 0.0001));
                });

                Step("Get https://ropsten.etherscan.io/tx/%BlockchainHash% Checking the transaction in bcn explorer. Status Status=Success To = 0x856924997fa22efad8dc75e83acfa916490989a4 Value = 0.0001 Ether", () =>
                {

                });

                Step("POST https://backoffice-test.lykkex.net/Mock/MailMock/Find Phrase: cashoutbil@autotest.com BO->Development->mail mocks", () => { });

                Step("POST https://backoffice-test.lykkex.net/Mock/MailMock/GetEmailBody Email: cashoutbil@autotest.com Id: % letterId % Take the newest letter", () => { });

                Step("Cash out complete You have successfully performed cash out of 0.0001 ETH!", () => { });
            }

            [Test]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            public void E2EOrdinaryCashOutNumerousTest()
            {
                string initialEtherBalance = "";
                string finalEtherBalance = "";
                string DESTNATION_ADDRESS = "0x856924997fa22efad8dc75e83acfa916490989a4";
                var EtherBalanceAPIURL = $"https://api-ropsten.etherscan.io/api?module=account&action=balance&address={DESTNATION_ADDRESS}&tag=latest";
                var walletBalance = 0d;
                var finalBalance = 0d;
                var assetId = "e58aa37d-dd46-4bdb-bac1-a6d0d44e6dc9";
                var signToken = "";
                var signVerToken = "";
                var pin = "1111";
                var signedVerificationToken = "";
                var WalletId = "da268259-c2f1-414a-96b1-e0fd5b9b724f";
                var zeroTransactionId = "";

                Step($"Get latest transaction id for wallet {WalletId}", () =>
                {
                    zeroTransactionId = GetLatestFinishedTransactionId(WalletId);
                });

                Step("Get initial balance of Ether waller: bcn initial balance = GET https://ropsten.etherscan.io/address/0x856924997fa22efad8dc75e83acfa916490989a4   Balance: % somebalance % Ether-> % inicialBcnBalance % ", () =>
                {
                    var balancePage = apiV2.CustomRequests.GetResponse(EtherBalanceAPIURL);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial ETHER balance");

                    initialEtherBalance = JsonConvert.DeserializeObject<EtherBalanceObjectModel>(balancePage.Content).result;
                });

                Step("Get initial balance of lykke client wallet. initial balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb - bac1 - a6d0d44e6dc9 assets.Balance-> % initialBalance % ", () =>
                {
                    var wallet = WalletApi.Wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    walletBalance = assetBalance.Balance;
                });

                Step("GET https://api-test.lykkex.net/api/signatureVerificationToken/KeyConfirmation?email=%email%", () =>
                {
                    var signTokenResponse = WalletApi.SignatureVerificationToken.GetKeyConfirmation(wallet.WalletAddress, BearerToken);
                    Assert.That(signTokenResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                    signToken = signTokenResponse.ResponseObject.Result.Message.ToString();
                    signVerToken = signToken;

                    //
                    WalletApi.PinSecurity.GetPinSecurity(pin, BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError("Please Reset Pin for user in Back Office");

                    WalletApi.Client.GetClientCodes(BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

                    string accessToken = WalletApi.Client.PostClientCodes(new SubmitCodeModel("0000"), BearerToken).Validate.StatusCode(HttpStatusCode.OK)
                        .Validate.NoApiError().GetResponseObject().Result.AccessToken;

                    string encodedPrivateKey = WalletApi.Client
                        .PostClientEncodedMainKey(new AccessTokenModel(accessToken), BearerToken)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result.EncodedPrivateKey;

                    string privateKeyStr = AesUtils.Decrypt(encodedPrivateKey, wallet.WalletKey);
                    Key privateKey = Key.Parse(privateKeyStr);
                    //

                    signToken = privateKey.SignMessage(signToken);
                });

                Step("POST https://api-test.lykkex.net/api/signatureVerificationToken/KeyConfirmation", () =>
                {
                    var postSignResponse = WalletApi.SignatureVerificationToken.PostKeyConfirmation(new RecoveryTokenChallangeResponse { Email = wallet.WalletAddress, SignedMessage = signToken }, BearerToken);
                    Assert.That(postSignResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    signedVerificationToken = postSignResponse.ResponseObject.Result.AccessToken;
                });

                Step("POST https://api-test.lykkex.net/api/HotWallet/cashout", () =>
                {
                    var postCashOut = WalletApi.HotWallet.PostCashOut(new HotWalletCashoutOperation
                    {
                        AssetId = assetId,
                        DestinationAddress = DESTNATION_ADDRESS,
                        DestinationAddressExtension = "",
                        Volume = 0.0001
                    }, signedVerificationToken, BearerToken);

                    Assert.That(postCashOut.StatusCode, Does.Not.EqualTo(HttpStatusCode.NotFound).And.Not.EqualTo(HttpStatusCode.InternalServerError).And.Not.EqualTo(HttpStatusCode.BadGateway), "Wrong Cashout Status code");

                    for (int i = 0; i < 2; i++)
                    {
                        signedVerificationToken = GetApi1SignVerToken();
                        postCashOut = WalletApi.HotWallet.PostCashOut(new HotWalletCashoutOperation
                        {
                            AssetId = assetId,
                            DestinationAddress = DESTNATION_ADDRESS,
                            DestinationAddressExtension = "",
                            Volume = 0.0001
                        }, signedVerificationToken, BearerToken);

                        Assert.That(postCashOut.StatusCode, Does.Not.EqualTo(HttpStatusCode.NotFound).And.Not.EqualTo(HttpStatusCode.InternalServerError).And.Not.EqualTo(HttpStatusCode.BadGateway), "Wrong Cashout Status code");
                    }
                });

                Step("Delay ~ 20min cashout confirmation and waiting fo Finished status", () =>
                {
                    if (!WaitForLatestHistoryResponseGotFinishedState(WalletId, TimeSpan.FromMinutes(20)))
                        Assert.Fail("Cashout comfirmation does not exist afer 20 minutes");
                    Assert.That(GetLatestFinishedTransactionId(WalletId), Does.Not.EqualTo(zeroTransactionId), "Latest transaction ID is the same as before test. History service does not know about our transaction. POST cashout failed?");
                });

                Step("Get Final Balance; final balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb  - bac1 - a6d0d44e6dc9 assets.Balance-> % finalBalance %      initialBalance + finalBalance = %Volume - %feeSize", () =>
                {
                    var wallet = WalletApi.Wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    finalBalance = assetBalance.Balance;
                });

                Step("bcn final balance = GET https://ropsten.etherscan.io/address/0x856924997fa22efad8dc75e83acfa916490989a4 . Validate that final balance on ether wallet is initial balance + 20* volume cashout ", () =>
                {
                    var balancePage = apiV2.CustomRequests.GetResponse(EtherBalanceAPIURL);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial ETHER balance");

                    finalEtherBalance = JsonConvert.DeserializeObject<EtherBalanceObjectModel>(balancePage.Content).result;

                    Assert.That(decimal.Parse(finalEtherBalance), Is.EqualTo(decimal.Parse(initialEtherBalance) + 22 * 100m));
                });
            }

            [TestCase("0x856924997fa22efad8dc75e83acfa916490989a4", true)]
            [TestCase("123456789", false)]
            [TestCase("0x403081CF29C318c34c2e2f9936cbd4DEf2550916", false)]
            [TestCase("0xc095146B97f7EcAdA2bf5E794D4F75f0e6E486f8", false)]
            [Category("ApiV2")]
            [Category("E2E")]
            [Category("ApiV2Operations")]
            public void DestinationAddressValidityApi1Test(string address, bool result)
            {
                var assetId = "e58aa37d-dd46-4bdb-bac1-a6d0d44e6dc9";

                Step("Make GET https://api-test.lykkex.net/api/HotWallet/addresses/${destination}/{assetId}/validity?addressExtension=  and validate response", () =>
                {
                    var response = WalletApi.HotWallet.GetHotWalletAddressValidity(address, assetId, "", BearerToken);
                    Assert.That(response.ResponseObject.Result.IsValid, Is.EqualTo(result), $"Unexected result for address {address}. Expected value is {result}");
                });
            }

            [TestCase("0x856924997fa22efad8dc75e83acfa916490989a4", true)]
            [TestCase("123456789", false)]
            [TestCase("0x403081CF29C318c34c2e2f9936cbd4DEf2550916", false)]
            [TestCase("0xc095146B97f7EcAdA2bf5E794D4F75f0e6E486f8", false)]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            public void DestinationAddressValidityApi2Test(string address, bool result)
            {
                var assetId = "e58aa37d-dd46-4bdb-bac1-a6d0d44e6dc9";

                Step("Make GET https://apiv2-test.lykkex.net/api/withdrawals/crypto  and validate response", () =>
                {
                    var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdValidateAddress(assetId, address, "", BearerToken);
                    Assert.That(response.ResponseObject.IsValid, Is.EqualTo(result), $"Unexected result for address {address}. Expected value is {result}");
                });
            }


            #region extensions

            #region models
            public class ExtensionBalanceResponse
            {
                public Result result { get; set; }
            }

            public class Result
            {
                public Account_Data account_data { get; set; }
                public int ledger_current_index { get; set; }
                public string status { get; set; }
                public bool validated { get; set; }
            }

            public class Account_Data
            {
                public string Account { get; set; }
                public string Balance { get; set; }
                public int Flags { get; set; }
                public string LedgerEntryType { get; set; }
                public int OwnerCount { get; set; }
                public string PreviousTxnID { get; set; }
                public int PreviousTxnLgrSeq { get; set; }
                public int Sequence { get; set; }
                public string index { get; set; }
            }
            #endregion

            [Test]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            [Category("E2E")]
            public void E2EOrdinaryFlowApi1ExtensionsTest()
            {
                string initialEtherBalance = "";
                string finalEtherBalance = "";
                string DESTNATION_ADDRESS = "rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY";
                var RippleBalanceAPIURL = $"https://s.altnet.rippletest.net:51234";
                var walletBalance = 0d;
                var finalBalance = 0d;
                var assetId = "c9cf27b2-9c68-4159-a30f-23a99950d929";
                var signToken = "";
                var signVerToken = "";
                var pin = "1111";
                var signedVerificationToken = "";
                var WalletId = "da268259-c2f1-414a-96b1-e0fd5b9b724f";
                var zeroTransactionId = "";
                var extension = "";
                var pkSecret = "sapoZtw4oZiwhoHvF6Z7thTGmpaJ6";
                var volume = 1m;

                //Step($"Get latest transaction id for wallet {WalletId}", () =>
                //{
                //    zeroTransactionId = GetLatestFinishedTransactionId(WalletId);
                //});

                Step("Get initial balance of Ripple waller: bcn initial balance = GET http://ripplerpc:!Lykke.2@52.232.122.53:5005/", () =>
                {
                    var json = "{\"method\": \"account_info\",\"params\": [{\"account\": \"rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY\" }]}";

                    var balancePage = apiV2.CustomRequests.PostResponse(RippleBalanceAPIURL, json);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial Ripple balance");

                    initialEtherBalance = JsonConvert.DeserializeObject<ExtensionBalanceResponse>(balancePage.Content).result.account_data.Balance;
                });

                Step("Get initial balance of lykke client wallet. initial balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb - bac1 - a6d0d44e6dc9 assets.Balance-> % initialBalance % ", () =>
                {
                    var wallet = WalletApi.Wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    walletBalance = assetBalance.Balance;
                });

                Step("GET https://api-test.lykkex.net/api/signatureVerificationToken/KeyConfirmation?email=%email%", () =>
                {
                    var signTokenResponse = WalletApi.SignatureVerificationToken.GetKeyConfirmation(wallet.WalletAddress, BearerToken);
                    Assert.That(signTokenResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                    signToken = signTokenResponse.ResponseObject.Result.Message.ToString();
                    signVerToken = signToken;

                    //
                    WalletApi.PinSecurity.GetPinSecurity(pin, BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError("Please Reset Pin for user in Back Office");

                    WalletApi.Client.GetClientCodes(BearerToken).Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();

                    string accessToken = WalletApi.Client.PostClientCodes(new SubmitCodeModel("0000"), BearerToken).Validate.StatusCode(HttpStatusCode.OK)
                        .Validate.NoApiError().GetResponseObject().Result.AccessToken;

                    string encodedPrivateKey = WalletApi.Client
                        .PostClientEncodedMainKey(new AccessTokenModel(accessToken), BearerToken)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result.EncodedPrivateKey;

                    string privateKeyStr = AesUtils.Decrypt(encodedPrivateKey, wallet.WalletKey);
                    Key privateKey = Key.Parse(privateKeyStr);
                    //

                    signToken = privateKey.SignMessage(signToken);
                });

                Step("POST https://api-test.lykkex.net/api/signatureVerificationToken/KeyConfirmation", () =>
                {
                    var postSignResponse = WalletApi.SignatureVerificationToken.PostKeyConfirmation(new RecoveryTokenChallangeResponse { Email = wallet.WalletAddress, SignedMessage = signToken }, BearerToken);
                    Assert.That(postSignResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                    signedVerificationToken = postSignResponse.ResponseObject.Result.AccessToken;
                });

                Step("POST https://api-test.lykkex.net/api/HotWallet/cashout", () =>
                {
                    var postCashOut = WalletApi.HotWallet.PostCashOut(new HotWalletCashoutOperation
                    {
                        AssetId = assetId,
                        DestinationAddress = DESTNATION_ADDRESS,
                        DestinationAddressExtension = "",
                        Volume = (double)volume
                    }, signedVerificationToken, BearerToken);

                    Assert.That(postCashOut.StatusCode, Does.Not.EqualTo(HttpStatusCode.NotFound).And.Not.EqualTo(HttpStatusCode.InternalServerError).And.Not.EqualTo(HttpStatusCode.BadGateway), "Wrong Cashout Status code");

                });

                Step("Delay ~ 20min cashout confirmation and waiting fo Finished status", () =>
                {
                    if (!WaitForLatestHistoryResponseGotFinishedState(WalletId, TimeSpan.FromMinutes(20)))
                        Assert.Fail("Cashout comfirmation does not exist afer 20 minutes");
                    Assert.That(GetLatestFinishedTransactionId(WalletId), Does.Not.EqualTo(zeroTransactionId), "Latest transaction ID is the same as before test. History service does not know about our transaction. POST cashout failed?");
                });

                Step("Get Final Balance; final balance = GET https://api-test.lykkex.net/api/Wallets assets.id = e58aa37d - dd46 - 4bdb  - bac1 - a6d0d44e6dc9 assets.Balance-> % finalBalance %      initialBalance + finalBalance = %Volume - %feeSize", () =>
                {
                    var wallet = WalletApi.Wallets.GetWallets(BearerToken);
                    var balances = apiV2.wallets.GetWalletsBalances(BearerToken).ResponseObject;
                    var walletBalances = balances.First().Balances.ToList();
                    var assetBalance = walletBalances.Find(b => b.AssetId == assetId);

                    finalBalance = assetBalance.Balance;
                });

                Step("bcn final balance = GET https://ropsten.etherscan.io/address/0x856924997fa22efad8dc75e83acfa916490989a4 . Validate that final balance on ether wallet is initial balance + volume cashout ", () =>
                {
                    var json = "{\"method\": \"account_info\",\"params\": [{\"account\": \"rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY\" }]}";

                    var balancePage = apiV2.CustomRequests.PostResponse(RippleBalanceAPIURL, json);
                    Assert.That(balancePage.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get initial Ripple balance");

                    Assert.That(() => decimal.Parse(
                        JsonConvert.DeserializeObject<ExtensionBalanceResponse>(apiV2.CustomRequests.PostResponse(RippleBalanceAPIURL, json).Content).result.account_data.Balance

                        ), Is.EqualTo(decimal.Parse(initialEtherBalance) + volume*(decimal)Math.Pow(10, 6)).After(3).Minutes.PollEvery(2).Seconds);
                });
            }

            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "", true)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "0", true)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "qweQWE123", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "0", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "3234881977", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "181272922", true)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY+", "111", false)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "111+", false)]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            [Category("E2E")]
            public void DestinationAddressValidityApi1ExtensionTest(string address, string extension, bool result)
            {
                var assetId = "c9cf27b2-9c68-4159-a30f-23a99950d929";

                Step($"Make GET https://api-test.lykkex.net/api/HotWallet/addresses/${address}/{assetId}/validity?addressExtension={extension}  and validate response", () =>
                {
                    var response = WalletApi.HotWallet.GetHotWalletAddressValidity(address, assetId, extension, BearerToken);
                    Assert.That(response.ResponseObject.Result.IsValid, Is.EqualTo(result), $"Unexected result for address {address}. Expected value is {result}");
                });
            }

            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "", true)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "0", true)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "qweQWE123", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "0", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "3234881977", false)]
            [TestCase("rE6jo1LZNZeD3iexQ6DnfCREEWZ9aUweVy", "181272922", true)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY+", "111", false)]
            [TestCase("rLUEUviNDaEU395WAdD7osWbzy6v9FqaMY", "111+", false)]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            [Category("E2E")]
            public void DestinationAddressValidityApi2ExtensionTest(string address, string extension, bool result)
            {
                var assetId = "c9cf27b2-9c68-4159-a30f-23a99950d929";
                Step("Make GET https://apiv2-test.lykkex.net/api/withdrawals/crypto  and validate response", () =>
                {
                    var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdValidateAddress(assetId, address, extension, BearerToken);
                    Assert.That(response.ResponseObject.IsValid, Is.EqualTo(result), $"Unexected result for address {address}. Expected value is {result}");
                });
            }

            #endregion
        }
    }
}
