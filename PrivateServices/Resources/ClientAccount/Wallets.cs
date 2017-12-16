using LykkeAutomationPrivate.Models.ClientAccount.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class Wallets: ClientAccountBase
    {
        public IResponse<WalletDto> PostCreateWallet(CreateWalletRequest wallet)
        {
            return Request.Post("api/Wallets").AddJsonBody(wallet).Build().Execute<WalletDto>();
        }

        public IResponse<WalletDto> GetWalletById(string id)
        {
            return Request.Get($"/api/Wallets/{id}").Build().Execute<WalletDto>();
        }

        public IResponse DeleteWalletById(string id)
        {
             return Request.Delete($"/api/Wallets/{id}").Build().Execute();
        }

        public IResponse<WalletDto> PutWalletById(string id, ModifyWalletRequest modifyWallet)
        {
            return Request.Put($"/api/Wallets/{id}").AddJsonBody(modifyWallet).Build().Execute<WalletDto>();
        }

        public IResponse<List<WalletDto>> GetWalletsForClientById(string id)
        {
            return Request.Get($"api/Wallets/client/{id}").Build().Execute<List<WalletDto>>();
        }

        public IResponse<List<WalletDto>> GetWalletsForClientByType(string id, WalletType walletType)
        {
            return Request.Get($"api/Wallets/client/{id}/type/{walletType}").Build().Execute<List<WalletDto>>();
        }
    }
}
