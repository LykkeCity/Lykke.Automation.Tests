using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Wallets : ApiBase
    {
        public IResponse<List<WalletModel>> GetWallets(string authorization)
        {
            return Request.Get("/wallets").WithBearerToken(authorization).Build().Execute<List<WalletModel>>();
        }

        public IResponse<WalletModel> PostWallets(CreateWalletRequest request, string authorization)
        {
            return Request.Post("/wallets").AddJsonBody(request).WithBearerToken(authorization).Build().Execute<WalletModel>();
        }

        public IResponse<CreateApiKeyResponse> PostWalletHFT(CreateApiKeyRequest request, string authorization)
        {
            return Request.Post("/wallets/hft").AddJsonBody(request).WithBearerToken(authorization).Build().Execute<CreateApiKeyResponse>();
        }

        public IResponse DeleteWallet(string walletId, string authorization)
        {
            return Request.Delete($"/wallets/{walletId}").WithBearerToken(authorization).Build().Execute();
        }

        public IResponse<WalletModel> GetWalletsId(string walletId, string authorization)
        {
            return Request.Get($"/wallets/{walletId}").WithBearerToken(authorization).Build().Execute<WalletModel>();
        }

        public IResponse<WalletModel> PutWalletsId(ModifyWalletRequest request, string walletId, string authorization)
        {
            return Request.Put($"/wallets/{walletId}").AddJsonBody(request).WithBearerToken(authorization).Build().Execute<WalletModel>();
        }

        public IResponse<List<WalletBalancesModel>> GetWalletsBalances(string authorization)
        {
            return Request.Get("/wallets/balances").WithBearerToken(authorization).Build().Execute<List<WalletBalancesModel>>();
        }

        public IResponse<List<ClientBalanceResponseModel>> GetWalletsTradingBalance(string authorization)
        {
            return Request.Get("/wallets/trading/balances").WithBearerToken(authorization).Build().Execute<List<ClientBalanceResponseModel>>();
        }

        public IResponse<List<ClientBalanceResponseModel>> GetWalletsBalanceAssetId(string walletId, string assetId, string authorization)
        {
            return Request.Get($"/wallets/{walletId}/balances/{assetId}").WithBearerToken(authorization).Build().Execute<List<ClientBalanceResponseModel>>();
        }
    }
}
