using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.ClientState
{
    class ClientStateTests
    {
        public class PostClientState : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientStateTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.ClientState.GetClientState(newUser.Email, newUser.PartnerId);

                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostClientStateOnlyEmail : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientStateOnlyEmailTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.ClientState.GetClientState(newUser.Email, null);

                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostClientStateOnlyPartnerId : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientStateOnlyPartnerIdTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.ClientState.GetClientState(null, newUser.PartnerId);

                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }

        public class PostClientStateEmpty : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientStateEmptyTest()
            {
                var response = walletApi.ClientState.GetClientState(null, null);

                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }
    }
}
