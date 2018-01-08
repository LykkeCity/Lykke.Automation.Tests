using LykkeAutomation.Api.ApiModels.ErrorModel;
using LykkeAutomation.ApiModels.PersonalDataModels;
using XUnitTestCommon.TestsData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LykkeAutomation.ApiModels.RegistrationModels
{
    public class AccountRegistrationModel
    {
            public string Email { get; set; }
            public string FullName { get; set; }
            public string ContactPhone { get; set; }
            public string Password { get; set; }
            public string Hint { get; set; }
            public string ClientInfo { get; set; }
            public string PartnerId { get; set; }

        public AccountRegistrationModel()
        {
            Email = TestData.GenerateEmail();
            FullName = TestData.FullName();
            Password = "654321";
            ContactPhone = "+71234567";
            Hint = "Holmes";
            ClientInfo = "<android>;Model:<LENOVO S860>;Os:<android>;Screen:<720x1184>";
            PartnerId = "None";
        }
    }
}
