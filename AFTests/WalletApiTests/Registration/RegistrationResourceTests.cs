﻿using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.TestsCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.Tests;

namespace AFTests.WalletApiTests
{
    class RegistrationResourceTests
    {

        public class PostRegistrationPositive : WalletApiBaseTest
        {
            [Test]
            [Category("Registration"), Category("WalletApi")]
            public void PostRegistrationPositiveTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var response = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                Assert.That(response.Error, Is.Null, $"Error message not empty {response.Error?.Message}");
                Assert.That(response.Result.PersonalData.FullName, Is.EqualTo(newUser.FullName), "FullName is not the same");
            }
        }
    }
}
