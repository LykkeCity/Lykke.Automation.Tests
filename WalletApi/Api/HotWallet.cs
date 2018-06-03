using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class HotWallet : ApiBase
    {
        public IResponse<ResponseModelHotWalletSuccessTradeRespModel> PostLimitOrder(
            HotWalletLimitOperation operation, string accessToken, string token) =>
            Request.Post("/HotWallet/limitOrder").WithBearerToken(token)
                .WithHeaders("SignatureVerificationToken", accessToken)
                .AddJsonBody(operation).Build().Execute<ResponseModelHotWalletSuccessTradeRespModel>();

        public IResponse<ResponseModelHotWalletSuccessTradeRespModel> PostMarketOrder(
            HotWalletOperation operation, string accessToken, string token) =>
            Request.Post("/HotWallet/marketOrder").WithBearerToken(token)
                .WithHeaders("SignatureVerificationToken", accessToken)
                .AddJsonBody(operation).Build().Execute<ResponseModelHotWalletSuccessTradeRespModel>();

        public IResponse<ResponseModel> PostCashOut(HotWalletCashoutOperation operation, string accessToken, string token) =>
            Request.Post("/HotWallet/cashout").WithBearerToken(token).WithHeaders("SignatureVerificationToken", accessToken)
                .AddJsonBody(operation).Build().Execute<ResponseModel>();

        //TODO: Add other
    }
}
