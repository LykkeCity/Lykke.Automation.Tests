using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.Registration.Models;
using TestsCore.RestRequests;

namespace LykkeAutomationPrivate.Resources.RegistrationResourse
{
    public class Registration
    {
        private string serviseUrl =
            EnvConfig.Env == Env.Test ? "http://registration.service.svc.cluster.local" :
            EnvConfig.Env == Env.Dev ? "http://registration.lykke-service.svc.cluster.local" :
            throw new Exception("Undefined env");

        public AccountsRegistrationResponseModel PostRegistration(AccountRegistrationModel account)
        {
            return Requests.For(serviseUrl).Post("api/Registration")
                .AddJsonBody(account).Build().Execute<AccountsRegistrationResponseModel>().GetResponseObject();
        }
    }
}
