using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.Models.Registration.Models;
using LykkeAutomationPrivate.Validators;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TestsCore.TestsData;

namespace LykkeAutomationPrivate.Tests.ClientAccount
{
    class GetClientAccountInformation : BaseTest
    {
        AccountRegistrationModel registration;
        ClientAccountInformationModel account;
        string pin = "1111";
        string partnerId = "NewTestPartner";

        [OneTimeSetUp]
        public void CreateClient()
        {
            registration = new AccountRegistrationModel().GetTestModel();
            registration.PartnerId = partnerId;
            account = lykkeApi.Registration.PostRegistration(registration).Account;
            lykkeApi.ClientAccount.ClientAccountInformation.PostSetPIN(account.Id, pin);
        }

        [Test]
        [Description("Get client by id.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientAccountInformationTest()
        {
            var getAccountInformation = lykkeApi.ClientAccount.ClientAccountInformation.GetClientAccountInformation(account.Id);
            Assert.That(getAccountInformation.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var accountInformation = getAccountInformation.GetResponseObject();
            Assert.That(accountInformation.Email, Is.EqualTo(registration.Email));
            Assert.That(accountInformation.Id, Is.EqualTo(account.Id));
            Assert.That(accountInformation.IsReviewAccount, Is.EqualTo(false));
            Assert.That(accountInformation.IsTrusted, Is.EqualTo(false));
            Assert.That(accountInformation.NotificationsId, Is.EqualTo(account.NotificationsId));
            Assert.That(accountInformation.PartnerId, Is.EqualTo(partnerId));
            Assert.That(accountInformation.Phone, Is.EqualTo(registration.ContactPhone));
            Assert.That(accountInformation.Pin, Is.EqualTo(pin));
            Assert.That(accountInformation.Registered, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(10)));
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
            var getClientsByPhone = lykkeApi.ClientAccount.ClientAccountInformation
                .GetClientsByPhone(registration.ContactPhone);
            Assert.That(getClientsByPhone.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(getClientsByPhone.GetResponseObject(), Does.Contain(account.Id));
        }

        [Test]
        [Description("Check if password is correct.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsPasswordCorrectTest()
        {
            var isPasswordCorrect = lykkeApi.ClientAccount.ClientAccountInformation
                .GetIsPasswordCorrect(account.Id, registration.Password);
            Assert.That(isPasswordCorrect.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(isPasswordCorrect.GetResponseObject(), Is.True);
        }

        [Test]
        [Description("Get client by id.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientByIdTest()
        {
            var client = lykkeApi.ClientAccount.ClientAccountInformation
                .GetClientById(account.Id);
            Assert.That(client.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var clientModel = client.GetResponseObject();
            Assert.That(clientModel.Id, Is.EqualTo(account.Id));
            Assert.That(clientModel.Email, Is.EqualTo(account.Email));
            Assert.That(clientModel.PartnerId, Is.EqualTo(partnerId));
            Assert.That(clientModel.Phone, Is.EqualTo(account.Phone));
            Assert.That(clientModel.Pin, Is.EqualTo(pin));
            Assert.That(clientModel.NotificationsId, Is.EqualTo(account.NotificationsId));
            Assert.That(clientModel.Registered, Is.EqualTo(account.Registered));
            Assert.That(clientModel.IsReviewAccount, Is.EqualTo(account.IsReviewAccount));
        }

        [Test]
        [Description("Get clients by email.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientsByEmailTest()
        {
            var client = lykkeApi.ClientAccount.ClientAccountInformation
                .GetClientsByEmail(account.Email);
            Assert.That(client.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(client.GetResponseObject().Count, Is.EqualTo(1));

            var clientModel = client.GetResponseObject().First();

            Assert.That(clientModel.Id, Is.EqualTo(account.Id));
            Assert.That(clientModel.Email, Is.EqualTo(account.Email));
            Assert.That(clientModel.PartnerId, Is.EqualTo(partnerId));
            Assert.That(clientModel.Phone, Is.EqualTo(account.Phone));
            Assert.That(clientModel.Pin, Is.EqualTo(pin));
            Assert.That(clientModel.NotificationsId, Is.EqualTo(account.NotificationsId));
            Assert.That(clientModel.Registered, Is.EqualTo(account.Registered));
            Assert.That(clientModel.IsReviewAccount, Is.EqualTo(account.IsReviewAccount));
        }

        [Test]
        [Description("Get client by email and partner id.")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientByEmailAndPartnerIdTest()
        {
            var client = lykkeApi.ClientAccount.ClientAccountInformation
                .GetClientByEmailAndPartnerId(account.Email, partnerId);
            Assert.That(client.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var clientModel = client.GetResponseObject();

            Assert.That(clientModel.Id, Is.EqualTo(account.Id));
            Assert.That(clientModel.Email, Is.EqualTo(account.Email));
            Assert.That(clientModel.PartnerId, Is.EqualTo(partnerId));
            Assert.That(clientModel.Phone, Is.EqualTo(account.Phone));
            Assert.That(clientModel.Pin, Is.EqualTo(pin));
            Assert.That(clientModel.NotificationsId, Is.EqualTo(account.NotificationsId));
            Assert.That(clientModel.Registered, Is.EqualTo(account.Registered));
            Assert.That(clientModel.IsReviewAccount, Is.EqualTo(account.IsReviewAccount));
        }
    }

    class Authenticate : BaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void AuthenticateExistedClientTest()
        {
            var registration = new AccountRegistrationModel().GetTestModel();
            //registration.PartnerId = partnerId;
            var  account = lykkeApi.Registration.PostRegistration(registration).Account;

            var clientAuthentication = new ClientAuthenticationModel()
            {
                Email = registration.Email,
                Password = registration.Password
            };
            var authenticate = lykkeApi.ClientAccount.ClientAccountInformation
                .PostAuthenticate(clientAuthentication);

            Assert.That(authenticate.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            AccountServiceValidator.Validate(registration, authenticate.GetResponseObject());
        }

        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void AuthenticateNonExistedClientTest()
        {
            var registration = new AccountRegistrationModel().GetTestModel();

            var clientAuthentication = new ClientAuthenticationModel()
            {
                Email = registration.Email,
                Password = registration.Password
            };
            var authenticate = lykkeApi.ClientAccount.ClientAccountInformation
                .PostAuthenticate(clientAuthentication);

            Assert.That(authenticate.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    class SetPin : BaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void SetPinTest()
        {
            var pin = "1111";

            var registration = new AccountRegistrationModel().GetTestModel();
            var account = lykkeApi.Registration.PostRegistration(registration).Account;

            var setPin = lykkeApi.ClientAccount.ClientAccountInformation.PostSetPIN(account.Id, pin);
            Assert.That(setPin.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(lykkeApi.ClientAccount.ClientAccountInformation.GetClientAccountInformation(account.Id)
                .GetResponseObject().Pin, Is.EqualTo(pin), "Wrong PIN");
        }
    }

    class ChangePassword : BaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Change client password")]
        public void ChangePasswordTest()
        {
            string newPassword = "987654321";
            var registration = new AccountRegistrationModel().GetTestModel();
            var account = lykkeApi.Registration.PostRegistration(registration).Account;

            var passwordHash = new PasswordHashModel()
            {
                ClientId = account.Id,
                PwdHash = Sha256.GenerateHash(newPassword)
            };

            var postChangeClientPassword = lykkeApi.ClientAccount.ClientAccountInformation
                .PostChangeClientPassword(passwordHash);
            Assert.That(postChangeClientPassword.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            //Is all ok?
            var newAuth = lykkeApi.ClientAccount.ClientAccountInformation
                .PostAuthenticate(new ClientAuthenticationModel()
                {
                    Email = registration.Email,
                    Password = newPassword
                });
            Assert.That(newAuth.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(newAuth.GetResponseObject().Id, Is.EqualTo(account.Id));

            //Check old password
            var oldPassAuth = lykkeApi.ClientAccount.ClientAccountInformation
                .PostAuthenticate(new ClientAuthenticationModel()
                {
                    Email = registration.Email,
                    Password = registration.Password
                });
            Assert.That(oldPassAuth.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }

    class ChangePhoneNumber : BaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Change client phone number.")]
        public void ChangePhoneNumberTest()
        {
            string newPhoneNumber = TestData.GeneratePhone();
            var registration = new AccountRegistrationModel().GetTestModel();
            var account = lykkeApi.Registration.PostRegistration(registration).Account;

            var postChangeClientPhoneNumber = lykkeApi.ClientAccount.ClientAccountInformation
                .PostChangeClientPhoneNumber(account.Id, newPhoneNumber);
            Assert.That(postChangeClientPhoneNumber.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var changedAccount = lykkeApi.ClientAccount.ClientAccountInformation
                .GetClientAccountInformation(account.Id);
            Assert.That(changedAccount.GetResponseObject().Phone, Is.EqualTo(newPhoneNumber));
        }
    }

    class InsertIndexedByPhoneAsync : BaseTest
    {
        [Test]
        [Ignore("Could not test this method")]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        [Description("Insert indexed by phone.")]
        public void InsertIndexedByPhoneAsynTest()
        {
            
        }
    }

    class PartnerIds : BaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void PartnerIdsTest()
        {
            AccountRegistrationModel registration;
            ClientAccountInformationModel account;
            string partnerId = "NewTestPartner";

            registration = new AccountRegistrationModel().GetTestModel();
            registration.PartnerId = partnerId;
            account = lykkeApi.Registration.PostRegistration(registration).Account;

            var partnerIds = lykkeApi.ClientAccount.ClientAccountInformation
                .PostPartnerIds(new EmailsModel()
                {
                    Values = new List<string> { account.Email }
                });

            Assert.That(partnerIds.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var response = partnerIds.JObject;
            Assert.That(response, Is.Not.Null);
            Assert.That(response[account.Id], Is.Not.Null);
            List<string> ids = ((JArray)response[account.Id]).Select(i => (string)i).ToList();
            Assert.That(ids, Does.Contain(partnerId));
        }
    }

    class ClientsByPhone : BaseTest
    {
        [Test]
        [Category("ClientAccountInformationResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostClientsByPhoneTest()
        {
            AccountRegistrationModel registration;
            ClientAccountInformationModel account;

            registration = new AccountRegistrationModel().GetTestModel();
            account = lykkeApi.Registration.PostRegistration(registration).Account;

            var clientsByPhone = lykkeApi.ClientAccount.ClientAccountInformation
                .PostClientsByPhone(new PhoneNumbersModel()
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
