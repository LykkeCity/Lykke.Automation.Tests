using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.Models.Registration.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LykkeAutomationPrivate.Validators
{
    public static class AccountServiceValidator
    {
        public static void Validate(AccountRegistrationModel accountRegistrationModel,
            ClientAccountInformation clientAccountInformation)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(accountRegistrationModel.Email, clientAccountInformation.Email, "Wrong email");
                Assert.AreEqual(accountRegistrationModel.ContactPhone, clientAccountInformation.Phone, "Wrong email");
            });
        }
    }
}
