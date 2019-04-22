using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Operations : ApiBase
    {
        public IResponse<OperationModel> GetOperationById(string id, string authorization)
        {
            return Request.Get($"/operations/{id}").WithBearerToken(authorization).Build().Execute<OperationModel>();
        }

        public IResponse PostOperationTransfer(CreateTransferRequest transferRequest, string id, string authorization)
        {
            return Request.Post($"/operations/transfer/{id}")
                .WithBearerToken(authorization)
                .AddJsonBody(transferRequest).Build().Execute();
        }

        public IResponse PostOperationCashOut(CreateCashoutRequest cashoutRequest, string id, string authorization)
        {
            return Request.Post($"operations/cashout/crypto/{id}")
                .WithBearerToken(authorization)
                .AddJsonBody(cashoutRequest).Build().Execute();
        }

        public IResponse PostOperationCancel(string id, string authorization)
        {
            return Request.Post($"/operations/cancel{id}")
                .WithBearerToken(authorization)
                .Build().Execute();
        }
    }
}
