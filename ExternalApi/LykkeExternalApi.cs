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
using TestsCore.RestRequests;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomation.Api
{
   public class LykkeExternalApi
    {
        protected string URL = "https://api-test.lykkex.net/api";

        protected IRequestBuilder Request => Requests.For(URL);

        public PersonalData PersonalData => new PersonalData();
        public Registration Registration => new Registration();
        public Auth Auth => new Auth();
        public AccountExist AccountExist => new AccountExist();
    }
}
