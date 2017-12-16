using LykkeAutomation.Api.AuthResource;
using LykkeAutomation.Api.PersonalDataResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LykkeAutomation.Api.RegistrationResource;
using LykkeAutomation.Api.ApiModels.AccountExistModels;
using LykkeAutomation.Api.ApiResources.AccountExist;
using TestsCore.ApiRestClient;

namespace LykkeAutomation.Api
{
   public class LykkeExternalApi
    {
        public PersonalData PersonalData => new PersonalData();
        public Registration Registration => new Registration();
        public Auth Auth => new Auth();
        public AccountExist AccountExist => new AccountExist();
    }

    public class ExternalRestApi : RestApi
    {
        public ExternalRestApi() : base("https://api-test.lykkex.net/api") { }

        public override void SetAllureProperties()
        {
        }
    }
}
