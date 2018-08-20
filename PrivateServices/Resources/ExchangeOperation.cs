using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace PrivateServices.Resources
{
    public class ExchangeOperation
    {

        private string serviseUrl =
            EnvConfig.Env == Env.Test ? "http://exchange-operations.service.svc.cluster.local/api" :
            EnvConfig.Env == Env.Dev ? "http://exchange-operations.lykke-service.svc.cluster.local/api" :
            throw new Exception("Undefined env");

        public IResponse<ExchangeOperationResult> PostManualCashIn(ManualCashInRequestModel cashIn)
        {
            return Requests.For(serviseUrl).Post("/ExchangeOperations/ManualCashIn")
                .AddJsonBody(cashIn).Build().Execute<ExchangeOperationResult>();
        }
    }
}
