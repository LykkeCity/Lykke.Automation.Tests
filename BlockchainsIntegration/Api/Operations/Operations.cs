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

        public IResponse<PostTransactionBroadcastResponse> PostTransactionsBroadcast(BroadcastTransactionRequest model)
        {
            return Request.Post("/transactions/broadcast").AddJsonBody(model).Build().Execute<PostTransactionBroadcastResponse>();
        }

        public IResponse DeleteOperationId(string operationId)
        {
            return Request.Delete($"/transactions/broadcast/{operationId}").Build().Execute();
        }

        public IResponse<BroadcastedTransactionResponse> GetOperationId(string operationId)
        {
            return Request.Get($"/transactions/broadcast/single/{operationId}").Build().Execute<BroadcastedTransactionResponse>();
        }

        public IResponse<TransactionsManyInputsResponse> PostTransactionsManyInputs(TransactionsManyInputsRequest model)
        {
            return Request.Post("/transactions/many-inputs").AddJsonBody(model).Build().Execute<TransactionsManyInputsResponse>();
        }

        public IResponse<TransactionsManyOutputsResponse> PostTransactionsManyOutputs(TransactionsManyOutputsRequest model)
        {
            return Request.Post("/transactions/many-outputs").Build().Execute<TransactionsManyOutputsResponse>();
        }

        public IResponse<GetTransactionsManyInputsResponse> GetTransactionsManyInputs(string operationId)
        {
            return Request.Post($"/transactions/many-inputs/{operationId}").Build().Execute<GetTransactionsManyInputsResponse>();
        }

        public IResponse<GetTransactionsManyOutputsResponse> GetTransactionsManyOutputs(string operationId)
        {
            return Request.Get($"/transactions/many-outputs/{operationId}").Build().Execute<GetTransactionsManyOutputsResponse>();
        }

        public IResponse<PutTransactionsResponse> PutTransactions(PutTransactionsRequest model)
        {
            return Request.Put("/transactions").AddJsonBody(model).Build().Execute<PutTransactionsResponse>();
        }

        public IResponse DeleteTranstactionsObservationFromAddress(string address)
        {
            return Request.Delete($"/transactions/history/from/{address}/observation").Build().Execute();
        }

        public IResponse DeleteTranstactionsObservationToAddress(string address)
        {
            return Request.Delete($"/transactions/history/to/{address}/observation").Build().Execute();
        }

        public IResponse<GetTransactionsHistoryFromToResponse> GetTransactionHistorFromAddress(string address, string take=null, string afterHash = null)
        {
            return Request.Get($"/api/transactions/history/to/{address}").AddQueryParameterIfNotNull("take", take)
                .AddQueryParameterIfNotNull("afterHash", afterHash).Build().Execute<GetTransactionsHistoryFromToResponse>();
        }

        public IResponse<GetTransactionsHistoryFromToResponse> GetTransactionHistorToAddress(string address, string take = null, string afterHash = null)
        {
            return Request.Get($"/api/transactions/history/to/{address}").AddQueryParameterIfNotNull("take", take)
                .AddQueryParameterIfNotNull("afterHash", afterHash).Build().Execute<GetTransactionsHistoryFromToResponse>();
        }
    }
}
