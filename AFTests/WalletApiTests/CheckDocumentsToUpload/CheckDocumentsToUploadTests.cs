using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.CheckDocumentsToUpload
{
    class CheckDocumentsToUploadTests
    {

        public class GetCheckDocumentsToUpload : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetCheckDocumentsToUploadTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.CheckDocumentsToUpload.GetCheckDocumentsToUpload(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null, "Unexpected error");
                Assert.That(response.GetResponseObject().Result.IdCard, Is.True, "Unexpected IdCard");
                Assert.That(response.GetResponseObject().Result.ProofOfAddress, Is.True, "Unexpected ProofOfAddress");
                Assert.That(response.GetResponseObject().Result.Selfie, Is.True, "Unexpected Selfie");
            }
        }

        public class GetCheckDocumentsToUploadInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("1234567")]
            [TestCase("!@$%^(9")]
            [Category("WalletApi")]
            public void GetCheckDocumentsToUploadInvalidTokenTest(string token)
            {
                var response = walletApi.CheckDocumentsToUpload.GetCheckDocumentsToUpload(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);               
            }
        }
    }
}