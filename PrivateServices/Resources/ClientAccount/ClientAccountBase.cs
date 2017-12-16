using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class ClientAccountBase
    {
        public string ServiseUrl =
            EnvConfig.Env == Env.Test ? "http://client-account.service.svc.cluster.local" :
            EnvConfig.Env == Env.Dev ? "http://client-account.lykke-service.svc.cluster.local" : 
            throw new Exception("Undefined env");

        protected IRequestBuilder Request => Requests.For(ServiseUrl);

        public Wallets Wallets => new Wallets();
        public AccountExist AccountExist => new AccountExist();
        public BannedClients BannedClients => new BannedClients();
        public ClientAccount ClientAccount => new ClientAccount();
        public ClientAccountInformationResource ClientAccountInformation => new ClientAccountInformationResource();
    }
}
