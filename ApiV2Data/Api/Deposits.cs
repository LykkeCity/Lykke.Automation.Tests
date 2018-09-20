using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Deposits : ApiBase
    {
        public IResponse<PaymentTransactionResponse> GetDepositsFXPayGateLast(string token)
        {
            return Request.Get("/Deposits/fxpaygate/last").WithBearerToken(token).Build().Execute<PaymentTransactionResponse>();
        }

        public IResponse<FxPaygateFeeModel> GetDepositsFXPayGateFee(string token)
        {
            return Request.Get("/Deposits/fxpaygate/fee").WithBearerToken(token).Build().Execute<FxPaygateFeeModel>();
        }

        public IResponse<FxPaygatePaymentUrlResponseModel> PostDepositsFXPayGate(FxPaygatePaymentUrlRequestModel model, string token)
        {
            return Request.Post("/Deposits/fxpaygate").AddJsonBody(model).WithBearerToken(token).Build().Execute<FxPaygatePaymentUrlResponseModel>();
        }

        public IResponse PostSwiftAssetIdEmail(SwiftDepositEmailModel model, string assetId, string token)
        {
            return Request.Post($"/Deposits/swift/{assetId}/email").AddJsonBody(model).WithBearerToken(token).Build().Execute();
        }

        public IResponse<SwiftRequisitesRespModel> GetSwiftAssetIdRequisites(string assetId, string token)
        {
            return Request.Get($"/Deposits/swift/{assetId}/requisites").WithBearerToken(token).Build().Execute<SwiftRequisitesRespModel>();
        }

        public IResponse<CryptoDepositAddressRespModel> GetCryptoAssetIdAddress(string assetId, string token)
        {
            return Request.Get($"/Deposits/crypto/{assetId}/address").WithBearerToken(token).Build().Execute<CryptoDepositAddressRespModel>();
        }

        public IResponse PostCryptoAssetIdAddress(string assetId, string token)
        {
            return Request.Post($"/Deposits/crypto/{assetId}/address").WithBearerToken(token).Build().Execute();
        }
    }
}
