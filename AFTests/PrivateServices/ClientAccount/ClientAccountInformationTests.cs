using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.PrivateApiTests
{
    class ClientAccountInformationBaseTest : PrivateApiBaseTest
    {
        protected ClientAccountInformationResource api;
        protected ClientRegistrationModel clientRegistration;
        protected ClientAccountInformation account;
        protected Partner partner;
        protected string pin = "1111";

        [OneTimeSetUp]
        public void CreatePartnerAndClientAndApi()
        {
            api = lykkeApi.ClientAccount.ClientAccountInformation;
            partner = new Partner().GetTestModel();
            lykkeApi.ClientAccount.Partners.PostPartners(partner);
            
            clientRegistration = new ClientRegistrationModel().GetTestModel(partner.PublicId);
            account = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration)
                .GetResponseObject();
            lykkeApi.ClientAccount.ClientAccountInformation.PostSetPIN(account.Id, pin);
            account.Pin = pin;
        }

        [OneTimeTearDown]
        public void RemovePartnerAndClient()
        {
            lykkeApi.ClientAccount.Partners.DeleteRemovePartner(partner.InternalId);
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(account.Id);
        }
    }

    class GetClientAccountInformation : ClientAccountInformationBaseTest
    {
        [Test]
        [Description("Get client by id.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientAccountInformationTest()
        {
            var getAccountInformation = api.GetClientAccountInformation(account.Id);
            Assert.That(getAccountInformation.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getAccountInformation.GetResponseObject(), Is.EqualTo(account));
        }

        [Test]
        [Ignore("Could not send body in GET request")]
        [Description("Get clients by ids.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientsByIdsTest()
        {

        }

        [Test]
        [Description("Get clients by phone.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientsByPhoneTest()
        {
            var getClientsByPhone = api.GetClientsByPhone(account.Phone);
            Assert.That(getClientsByPhone.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getClientsByPhone.GetResponseObject(), Does.Contain(account.Id));
        }

        [Test]
        [Description("Check if password is correct.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsPasswordCorrectTest()
        {
            var isPasswordCorrect = api.GetIsPasswordCorrect(account.Id, clientRegistration.Password);
            Assert.That(isPasswordCorrect.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(isPasswordCorrect.GetResponseObject(), Is.True);
        }

        [Test]
        [Description("Get client by id.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientByIdTest()
        {
            var getClient = api.GetClientById(account.Id);
            Assert.That(getClient.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getClient.GetResponseObject(), Is.EqualTo(account));
        }

        [Test]
        [Description("Get clients by email.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientsByEmailTest()
        {
            var getClient = api.GetClientsByEmail(account.Email);
            Assert.That(getClient.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getClient.GetResponseObject(), Has.Count.EqualTo(1).And.Contain(account));
        }

        [Test]
        [Description("Get client by email and partner id.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientByEmailAndPartnerIdTest()
        {
            var getClient = api.GetClientByEmailAndPartnerId(account.Email, partner.PublicId);
            Assert.That(getClient.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getClient.GetResponseObject(), Is.EqualTo(account));
        }
    }

    class Authenticate : ClientAccountInformationBaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void AuthenticateExistedClientTest()
        {
            var clientAuthentication = new ClientAuthenticationModel()
            {
                Email = clientRegistration.Email,
                Password = clientRegistration.Password,
                ParantId = partner.PublicId
            };
            var postAuthenticate = api.PostAuthenticate(clientAuthentication);
            Assert.That(postAuthenticate.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postAuthenticate.GetResponseObject(), Is.EqualTo(account));
        }

        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void AuthenticateNonExistedClientTest()
        {
            var registration = new ClientRegistrationModel().GetTestModel(partner.PublicId);
            var clientAuthentication = new ClientAuthenticationModel()
            {
                Email = registration.Email,
                Password = registration.Password,
                ParantId = partner.PublicId
            };
            var authenticate = api.PostAuthenticate(clientAuthentication);

            Assert.That(authenticate.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    class SetPin : ClientAccountInformationBaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void SetPinTest()
        {
            var newPin = "1234";

            var postSetPin = api.PostSetPIN(account.Id, newPin);
            Assert.That(postSetPin.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(api.GetClientAccountInformation(account.Id)
                .GetResponseObject().Pin, Is.EqualTo(newPin), "Wrong PIN");
        }
    }

    class ChangePassword : ClientAccountInformationBaseTest
    {
        string newPassword = "987654321";

        [Test]
        [Order(1)]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Change client password")]
        public void ChangePasswordTest()
        {
            var passwordHash = new PasswordHashModel()
            {
                ClientId = account.Id,
                PwdHash = Sha256.GenerateHash(newPassword)
            };

            var postChangeClientPassword = api.PostChangeClientPassword(passwordHash);
            Assert.That(postChangeClientPassword.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Client could login with new password")]
        public void ChangePasswordCouldLoginTest()
        {
            var newAuth = api.PostAuthenticate(new ClientAuthenticationModel()
            {
                Email = clientRegistration.Email,
                Password = newPassword,
                ParantId = partner.PublicId
            });
            Assert.That(newAuth.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(newAuth.GetResponseObject(), Is.EqualTo(account));
        }

        [Test]
        [Order(3)]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Client could not login with old password")]
        public void ChangePasswordCouldNotLoginWithOldPasswordTest()
        {
            var oldPassAuth = api.PostAuthenticate(new ClientAuthenticationModel()
            {
                Email = clientRegistration.Email,
                Password = clientRegistration.Password,
                ParantId = partner.PublicId
            });
            Assert.That(oldPassAuth.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    class ChangePhoneNumber : ClientAccountInformationBaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Change client phone number.")]
        public void ChangePhoneNumberTest()
        {
            string newPhoneNumber = TestData.GeneratePhone();

            var postChangeClientPhoneNumber = api.PostChangeClientPhoneNumber(account.Id, newPhoneNumber);
            Assert.That(postChangeClientPhoneNumber.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetClientAccountInformation(account.Id)
                .GetResponseObject().Phone, Is.EqualTo(newPhoneNumber));
        }
    }

    class InsertIndexedByPhoneAsync : ClientAccountInformationBaseTest
    {
        [Test]
        [Ignore("Could not test this method")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Insert indexed by phone.")]
        public void InsertIndexedByPhoneAsynTest()
        {
            
        }
    }

    class PartnerIds : ClientAccountInformationBaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void PartnerIdsTest()
        {
            var postPartnerIds = api.PostPartnerIds(new EmailsModel()
            {
                Values = new List<string> { account.Email }
            });

            Assert.That(postPartnerIds.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var response = postPartnerIds.JObject;
            Assert.That(response, Is.Not.Null);
            Assert.That(response[account.Id], Is.Not.Null);
            List<string> ids = ((JArray)response[account.Id]).Select(i => (string)i).ToList();
            Assert.That(ids, Does.Contain(partner.PublicId));
        }
    }

    class ClientsByPhone : ClientAccountInformationBaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostClientsByPhoneTest()
        {
            var clientsByPhone = api.PostClientsByPhone(new PhoneNumbersModel()
            {
                Values = new List<string> { account.Phone }
            });

            Assert.That(clientsByPhone.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var response = clientsByPhone.JObject;
            Assert.That(response, Is.Not.Null);
            Assert.That((string)response[account.Id], Is.EqualTo(account.Phone));
        }
    }
}
