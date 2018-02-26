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

        public IResponse<BuildTransactionResponse> PostTransactions(BuildSingleTransactionRequest model)
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

        public IResponse<BroadcastedSingleTransactionResponse> GetOperationId(string operationId)
        {
            return Request.Get($"/transactions/broadcast/single/{operationId}").Build().Execute<BroadcastedSingleTransactionResponse>();
        }

        public IResponse<BuildTransactionResponse> PostTransactionsManyInputs(BuildTransactionWithManyInputsRequest model)
        {
            return Request.Post("/transactions/many-inputs").AddJsonBody(model).Build().Execute<BuildTransactionResponse>();
        }

        public IResponse<BuildTransactionResponse> PostTransactionsManyInputs(string model)
        {
            return Request.Post("/transactions/many-inputs").AddJsonBody(model).Build().Execute<BuildTransactionResponse>();
        }

        public IResponse<BuildTransactionResponse> PostTransactionsManyOutputs(BuildTransactionWithManyOutputsRequest model)
        {
            return Request.Post("/transactions/many-outputs").AddJsonBody(model).Build().Execute<BuildTransactionResponse>();
        }

        public IResponse<BroadcastedTransactionWithManyInputsResponse> GetTransactionsManyInputs(string operationId)
        {
            return Request.Get($"/transactions/broadcast/many-inputs/{operationId}").Build().Execute<BroadcastedTransactionWithManyInputsResponse>();
        }

        public IResponse<BroadcastedTransactionWithManyOutputsResponse> GetTransactionsManyOutputs(string operationId)
        {
            return Request.Get($"/transactions/broadcast/many-outputs/{operationId}").Build().Execute<BroadcastedTransactionWithManyOutputsResponse>();
        }

        public IResponse<PutTransactionsResponse> PutTransactions(RebuildTransactionRequest model)
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

        public IResponse<GetTransactionsHistoryFromToResponse> GetTransactionHistorFromAddress(string address, string take, string afterHash = null)
        {
            return Request.Get($"/transactions/history/to/{address}").AddQueryParameterIfNotNull("take", take)
                .AddQueryParameterIfNotNull("afterHash", afterHash).Build().Execute<GetTransactionsHistoryFromToResponse>();
        }

        public IResponse<GetTransactionsHistoryFromToResponse> GetTransactionHistorToAddress(string address, string take = null, string afterHash = null)
        {
            return Request.Get($"/transactions/history/to/{address}").AddQueryParameterIfNotNull("take", take)
                .AddQueryParameterIfNotNull("afterHash", afterHash).Build().Execute<GetTransactionsHistoryFromToResponse>();
        }

        public IResponse PostHistoryFromToAddress(string fromTo, string address)
        {
            return Request.Post($"/transactions/history/{fromTo}/{address}/observation").Build().Execute();
        }
    }
}
