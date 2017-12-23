using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.Registration.Models;
using System.Net;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AFTests.PrivateApiTests
{
    class WalletResourceTests : PrivateApiBaseTest
    {
        string userId;

        [OneTimeSetUp]
        public void CreateUser()
        {
            var client = new AccountRegistrationModel().GetTestModel();
            var registeredclient = lykkeApi.Registration.PostRegistration(client);
            userId = registeredclient.Account.Id;
            lykkeApi.ClientAccount.ClientAccountInformation.PostSetPIN(userId, "1111");
        }

        [Test]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostCreateWallet()
        {
            CreateWalletRequest createWalletRequest = new CreateWalletRequest().GetTestModel(userId);
            var postWalletPesp = lykkeApi.ClientAccount.Wallets.PostCreateWallet(createWalletRequest);
            var wallet = postWalletPesp.GetResponseObject();

            Assert.That(wallet.Id, Is.Not.Null);
            //Assert.That(wallet.Type, Is.EqualTo(createWalletRequest.Type.ToSerializedValue()));
            Assert.That(wallet.Name, Is.EqualTo(createWalletRequest.Name));
            Assert.That(wallet.Description, Is.EqualTo(createWalletRequest.Description));
        }

        [TestCase("5000000-aaaa-bbbb-cccc-5555aaa111000")]
        [TestCase("oloasdakj jashdasdjal asdjuasdasa")]
        [TestCase("")]
        [TestCase(null)]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostCreateWalletClientId(string _userId)
        {
            CreateWalletRequest createWalletRequest = new CreateWalletRequest().GetTestModel(_userId);
            var postWalletPesp = lykkeApi.ClientAccount.Wallets.PostCreateWallet(createWalletRequest);

            Assert.That(postWalletPesp.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        public class CreateWalletRequestMock : CreateWalletRequest
        {
            public new object Type { get; set; }
        }

        [TestCase(WalletType.Trading, ExpectedResult = HttpStatusCode.OK)]
        [TestCase(WalletType.Trusted, ExpectedResult = HttpStatusCode.OK)]
        [TestCase("SomeType", ExpectedResult = HttpStatusCode.BadRequest)]
        [TestCase("", ExpectedResult = HttpStatusCode.BadRequest)]
        [TestCase(null, ExpectedResult = HttpStatusCode.BadRequest)]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public HttpStatusCode PostCreateWalletType(object _walletType)
        {
            var clientAccountUrl = lykkeApi.ClientAccount.ServiseUrl;
            var walletResponse = Requests.For(clientAccountUrl).Post("/api/Wallets")
                .AddJsonBody(new CreateWalletRequestMock
                {
                    ClientId = userId,
                    Name = "Some test name",
                    Type = _walletType,
                    Description = "Some test description"
                })
                .Build().Execute();

            return walletResponse.StatusCode;
        }

        [TestCase("Some long-long-long maybe or not name...", ExpectedResult = HttpStatusCode.OK)]
        [TestCase("N", ExpectedResult = HttpStatusCode.OK)]
        [TestCase("", ExpectedResult = HttpStatusCode.BadRequest)]
        [TestCase(null, ExpectedResult = HttpStatusCode.BadRequest)]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public HttpStatusCode PostCreateWalletName(string _walletName)
        {
            CreateWalletRequest createWalletRequest = new CreateWalletRequest().GetTestModel(userId);
            createWalletRequest.Name = _walletName;
            var postWalletPesp = lykkeApi.ClientAccount.Wallets.PostCreateWallet(createWalletRequest);

            return postWalletPesp.StatusCode;
        }

        [TestCase("Some long-long-long maybe or not description", ExpectedResult = HttpStatusCode.OK)]
        [TestCase("", ExpectedResult = HttpStatusCode.OK)]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public HttpStatusCode PostCreateWalletDescription(string _walletDescription)
        {
            CreateWalletRequest createWalletRequest = new CreateWalletRequest().GetTestModel(userId);
            createWalletRequest.Description = _walletDescription;

            var postWalletPesp = lykkeApi.ClientAccount.Wallets.PostCreateWallet(createWalletRequest);

            return postWalletPesp.StatusCode;
        }

        [Test]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetWallet()
        {
            var walletToCreate = new CreateWalletRequest().GetTestModel(userId);
            var createdWalet = lykkeApi.ClientAccount.Wallets.PostCreateWallet(walletToCreate).GetResponseObject();

            var getWalletById = lykkeApi.ClientAccount.Wallets.GetWalletById(createdWalet.Id);

            Assert.That(getWalletById.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var recievedWallet = getWalletById.GetResponseObject();
            Assert.That(recievedWallet.Id, Is.EqualTo(createdWalet.Id));
            Assert.That(recievedWallet.Name, Is.EqualTo(createdWalet.Name));
            Assert.That(recievedWallet.Type, Is.EqualTo(createdWalet.Type));
            Assert.That(recievedWallet.Description, Is.EqualTo(createdWalet.Description));
        }

        [Test]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteWallet()
        {
            var walletToCreate = new CreateWalletRequest().GetTestModel(userId);
            var createdWalet = lykkeApi.ClientAccount.Wallets.PostCreateWallet(walletToCreate).GetResponseObject();

            var deleteWalletById = lykkeApi.ClientAccount.Wallets.DeleteWalletById(createdWalet.Id);
            Assert.That(deleteWalletById.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getWalletById = lykkeApi.ClientAccount.Wallets.GetWalletById(createdWalet.Id);
            Assert.That(getWalletById.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutWallet()
        {
            var walletToCreate = new CreateWalletRequest().GetTestModel(userId);
            var createdWalet = lykkeApi.ClientAccount.Wallets.PostCreateWallet(walletToCreate).GetResponseObject();

            var newWallet = new ModifyWalletRequest().GetTestModel();

            var putWalletById = lykkeApi.ClientAccount.Wallets.PutWalletById(createdWalet.Id, newWallet);
            Assert.That(putWalletById.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var changedWallet = lykkeApi.ClientAccount.Wallets.GetWalletById(createdWalet.Id).GetResponseObject();
            //Have not to change
            Assert.That(changedWallet.Id, Is.EqualTo(createdWalet.Id));
            Assert.That(changedWallet.Type, Is.EqualTo(createdWalet.Type));
            //Have to be changed
            Assert.That(changedWallet.Name, Is.EqualTo(newWallet.Name));
            Assert.That(changedWallet.Description, Is.EqualTo(newWallet.Description));
        }
    }

    class GetWalletsForClient : PrivateApiBaseTest
    {
        [Test]
        [Description("Get all existing wallets for client.")]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetWalletsForClientTest()
        {
            //create new client
            var client = new AccountRegistrationModel().GetTestModel();
            var registeredclient = lykkeApi.Registration.PostRegistration(client);
            var userId = registeredclient.Account.Id;
            lykkeApi.ClientAccount.ClientAccountInformation.PostSetPIN(userId, "1111");

            //Create 3 wallets
            var createdWalet1 = lykkeApi.ClientAccount.Wallets.PostCreateWallet(new CreateWalletRequest().GetTestModel(userId)).GetResponseObject();
            var createdWalet2 = lykkeApi.ClientAccount.Wallets.PostCreateWallet(new CreateWalletRequest().GetTestModel(userId)).GetResponseObject();
            var createdWalet3 = lykkeApi.ClientAccount.Wallets.PostCreateWallet(new CreateWalletRequest().GetTestModel(userId)).GetResponseObject();

            var getWalletsForClientById = lykkeApi.ClientAccount.Wallets.GetWalletsForClientById(userId).GetResponseObject();

            Assert.That(getWalletsForClientById.Count, Is.EqualTo(4));
            Assert.That(getWalletsForClientById.Select(w => w.Id),
                Does.Contain(createdWalet1.Id).And.Contain(createdWalet2.Id).And.Contain(createdWalet3.Id));
            Assert.That(getWalletsForClientById.Select(w => w.Name),
                Does.Contain(createdWalet1.Name).And.Contain(createdWalet2.Name).And.Contain(createdWalet3.Name));
            Assert.That(getWalletsForClientById.Select(w => w.Type),
                Does.Contain(createdWalet1.Type).And.Contain(createdWalet2.Type).And.Contain(createdWalet3.Type));
            Assert.That(getWalletsForClientById.Select(w => w.Description),
                Does.Contain(createdWalet1.Description).And.Contain(createdWalet2.Description).And.Contain(createdWalet3.Description));
        }
    }

    class GetWalletsOfTypeForClient : PrivateApiBaseTest
    {
        [Test]
        [Description("Get client wallets of provided type.")]
        [Category("Wallets"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetWalletsOfTypeForClientTest()
        {
            //create new client
            var client = new AccountRegistrationModel().GetTestModel();
            var registeredclient = lykkeApi.Registration.PostRegistration(client);
            var clientId = registeredclient.Account.Id;
            lykkeApi.ClientAccount.ClientAccountInformation.PostSetPIN(clientId, "1111");

            //Create 3 wallets
            var tradingWallet1 = new CreateWalletRequest().GetTestModel(clientId);
            tradingWallet1.Type = WalletType.Trading;
            var trustedWallet2 = new CreateWalletRequest().GetTestModel(clientId);
            trustedWallet2.Type = WalletType.Trusted;
            var trustedWallet3 = new CreateWalletRequest().GetTestModel(clientId);
            trustedWallet3.Type = WalletType.Trusted;

            var createdTradingWallet1 = lykkeApi.ClientAccount.Wallets
                .PostCreateWallet(tradingWallet1).GetResponseObject();
            var createdTrustedWalet2 = lykkeApi.ClientAccount.Wallets
                .PostCreateWallet(trustedWallet2).GetResponseObject();
            var createdTrustedWalet3 = lykkeApi.ClientAccount.Wallets
                .PostCreateWallet(trustedWallet3).GetResponseObject();

            var tradingWallets = lykkeApi.ClientAccount.Wallets
                .GetWalletsForClientByType(clientId, WalletType.Trading).GetResponseObject();

            var trustedWallets = lykkeApi.ClientAccount.Wallets
                .GetWalletsForClientByType(clientId, WalletType.Trusted).GetResponseObject();

            Assert.That(tradingWallets.Select(w => w.Id),
                Does.Contain(createdTradingWallet1.Id));

            Assert.That(trustedWallets.Select(w => w.Id),
                Does.Contain(createdTrustedWalet2.Id).And.Contain(createdTrustedWalet3.Id));
        }
    }
}