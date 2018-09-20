using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class History : ApiBase
    {

        public IResponse<GetClientHistoryCsvResponseModel> GetHistoryClientCSV(string id, string token)
        {
            return Request.Get("/History/client/csv").AddQueryParameter("id", id).WithBearerToken(token).Build().Execute<GetClientHistoryCsvResponseModel>();
        }

        public IResponse<RequestClientHistoryCsvResponseModel> PostHistoryClientCSV(RequestClientHistoryCsvRequestModel model, string token)
        {
            return Request.Post("/History/client/csv").AddJsonBody(model).WithBearerToken(token).Build().Execute<RequestClientHistoryCsvResponseModel>();
        }

        public IResponse<List<HistoryResponseModel>> GetHistoryWalletWalletId(string walletId, string operationType, string assetId, string assetPairId, string take, string skip, string token)
        {
            return Request.Get($"/History/wallet/{walletId}").AddQueryParameterIfNotNull("operationType", operationType).AddQueryParameterIfNotNull("assetId", assetId).AddQueryParameterIfNotNull("assetPairId", assetPairId).AddQueryParameterIfNotNull("take", take).AddQueryParameterIfNotNull("skip", skip).WithBearerToken(token).Build().Execute<List<HistoryResponseModel>>();
        }

        public IResponse<List<HistoryResponseModel>> GetHistoryWalletIdTrades(string walletId, string assetPairId, string take, string skip, string token)
        {
            return Request.Get($"/History/{walletId}/trades").AddQueryParameterIfNotNull("assetPairId", assetPairId).AddQueryParameterIfNotNull("take", take).AddQueryParameterIfNotNull("skip", skip).WithBearerToken(token).Build().Execute<List<HistoryResponseModel>>();
        }

        public IResponse<List<HistoryWalletFundsResponse>> GetHistoryWalletIdFunds(string walletId, string operation, string assetId, string take, string skip, string token)
        {
            return Request.Get($"/History/{walletId}/funds").AddQueryParameterIfNotNull("operation", operation).AddQueryParameterIfNotNull("assetId", assetId).AddQueryParameterIfNotNull("take", take).AddQueryParameterIfNotNull("skip", skip).WithBearerToken(token).Build().Execute<List<HistoryWalletFundsResponse>>();
        }
    }
}
