using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.TestsData;

namespace Lykke.Client.AutorestClient.Models
{
    public partial class AccountRegistrationModel
    {
        public AccountRegistrationModel GetTestModel(string partnerId = null)
        {
            return new AccountRegistrationModel()
            {
                Email = TestData.GenerateEmail(),
                FullName = TestData.FullName(),
                Password = "654321",
                ContactPhone = "+71234567",
                Hint = "Holmes",
                ClientInfo = "<android>;Model:<LENOVO S860>;Os:<android>;Screen:<720x1184>",
                PartnerId = partnerId
            };
        }
    }
}
