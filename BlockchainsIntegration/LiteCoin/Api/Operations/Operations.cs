using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LiteCoin.Api
{
    public class Operations : ApiBase
    {
        public IResponse<BuildTransactionRequest> PostTransactions(BuildTransactionRequest model)
        {
            return Request.Post("/transactions").AddJsonBody(model).Build().Execute<BuildTransactionRequest>();
        }

        public IResponse PostTransactionsBroadcast(string OperationId, string SignedTransaction)
        {
            return Request.Post("/transactions/broadcast").AddQueryParameterIfNotNull("OperationId", OperationId).
                AddQueryParameterIfNotNull("SignedTransaction", SignedTransaction).Build().Execute();
        }

        public IResponse DeleteOperationId(string operationId)
        {
            return Request.Delete($"/broadcast/{operationId}").Build().Execute();
        }

        public IResponse<BroadcastedTransactionResponse> GetOperationId(string operationId)
        {
            return Request.Get($"/broadcast/{operationId}").Build().Execute<BroadcastedTransactionResponse>();
        }
    }
}
