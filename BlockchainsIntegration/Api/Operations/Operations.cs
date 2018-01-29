using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LiteCoin.Api
{
    public class Operations : ApiBase
    {
        public Operations(string url) : base(url) { }

        public Operations() : base() { }

        public IResponse<BuildTransactionResponse> PostTransactions(BuildTransactionRequest model)
        {
            return Request.Post("/transactions/single").AddJsonBody(model).Build().Execute<BuildTransactionResponse>();
        }

        public IResponse PostTransactionsBroadcast(BroadcastTransactionRequest model)
        {
            return Request.Post("/transactions/broadcast").AddJsonBody(model).Build().Execute();
        }

        public IResponse DeleteOperationId(string operationId)
        {
            return Request.Delete($"/transactions/broadcast/{operationId}").Build().Execute();
        }

        public IResponse<BroadcastedTransactionResponse> GetOperationId(string operationId)
        {
            return Request.Get($"/transactions/broadcast/{operationId}").Build().Execute<BroadcastedTransactionResponse>();
        }

        public IResponse<TransactionsManyInputsResponse> PostTransactionsManyInputs(TransactionsManyInputsRequest model)
        {
            return Request.Post("/api/transactions/many-inputs").AddJsonBody(model).Build().Execute<TransactionsManyInputsResponse>();
        }

        public IResponse PostTransactionsManyOutputs(TransactionsManyInputsRequest model)
        {
            return Request.Post("/api/transactions/many-outputs").Build().Execute();
        }
    }
}
