using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BitcoinCash : ApiBase
    {
        string resource = "/BitcoinCash";

        public IResponse<ResponseModelBccMultisigTransactionResponseModel> GetBitcoinCashMultiSigBalance(string token)
        {
            return Request.Get(resource + "/multisig/balance").WithBearerToken(token).Build().Execute<ResponseModelBccMultisigTransactionResponseModel>();
        }

        public IResponse<ResponseModelBccTransactionResponseModel> GetBitcoinCashMultiSigTransaction(string token)
        {
            return Request.Get(resource + "/multisig/balance").WithBearerToken(token).Build().Execute<ResponseModelBccTransactionResponseModel>();
        }

        public IResponse<ResponseModelBccPrivateBalanceModel> GetBitcoinCashPrivateBalance(string address, string token)
        {
            return Request.Get(resource + "/private/balance").WithBearerToken(token).Build().Execute<ResponseModelBccPrivateBalanceModel>();
        }

        public IResponse<ResponseModelBccTransactionResponseModel> GetBitcoinCashPrivateTransaction(string sourceAddress, string destinationAddress, double fee, string token)
        {
            return Request.Get(resource + "/private/balance").WithBearerToken(token).Build().Execute<ResponseModelBccTransactionResponseModel>();
        }

        public IResponse<ResponseModelBccTransactionResponseModel> PostBitcoinCashBroadcast(BccBroadcastRequest model, string token)
        {
            return Request.Post(resource + "/broadcast").WithBearerToken(token).AddJsonBody(model).Build().Execute<ResponseModelBccTransactionResponseModel>();
        }
    }
}
