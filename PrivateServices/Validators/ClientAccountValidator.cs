using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.Models.Registration.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LykkeAutomationPrivate.Validators
{
    public static class ClientAccountValidator
    {
        public static void Validate(AccountRegistrationModel accountRegistration,
            ClientAccountInformation clientAccountInformation)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(accountRegistration.Email, clientAccountInformation.Email, "Wrong email");
                Assert.AreEqual(accountRegistration.ContactPhone, clientAccountInformation.Phone, "Wrong phone");
            });
        }

        public static void Validate(ClientRegistrationModel clientRegistration, ClientAccountInformation clientAccountInformation)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(clientRegistration.Email, clientAccountInformation.Email, "Wrong email");
                Assert.AreEqual(clientRegistration.Phone, clientAccountInformation.Phone, "Wrong email");
                Assert.AreEqual(clientRegistration.PartnerId, clientAccountInformation.PartnerId, "Wrong PartnerId");
            });
        }
    }
}
