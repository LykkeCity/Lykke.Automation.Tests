using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace PrivateServices.Resources
{
    public class Assets
    {
        public string ServiseUrl =
            EnvConfig.Env == Env.Test ? "http://assets.service.svc.cluster.local/api" :
            EnvConfig.Env == Env.Dev ? "http://assets.lykke-service.svc.cluster.local/api" :
            throw new Exception("Undefined env");

        public IResponse<Asset[]> GetAssets() =>
            Requests.For(ServiseUrl).Get("/v2/assets").Build().Execute<Asset[]>();
    }
}
