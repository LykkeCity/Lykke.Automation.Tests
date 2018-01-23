using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.BackupCompleted
{
    class BackupCompletedTests
    {
        public class BackupCompleted : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void BackupCompletedTest()
            {
                var user = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var response = walletApi.BackupCompleted.PostBackupCompleted(registeredClient.GetResponseObject().Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class BackupCompletedInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("123456789")]
            [TestCase("testtest")]
            [TestCase("!@$%^")]
            [Category("WalletApi")]
            public void BackupCompletedInvalidTokenTest(string token)
            {
                var response = walletApi.BackupCompleted.PostBackupCompleted(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
