using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.BankCardPaymentUrlFormValues
{
    class BankCardPaymentUrlFormValuesTests
    {
        public class GetBankCardPaymentUrlFormValues : WalletApiBaseTest
        {
            [Test]
            [Category("")]
            public void GetBankCardPaymentUrlFormValuesTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BankCardPaymentUrlFormValues.GetBankCardPaymentUrlFormValues(registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }
    }
}
