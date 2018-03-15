using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Client : ApiBase
    {
        string resource = "/Client";

        public IResponse<ResponseModel> GetClientCodes(string token)
        {
            return Request.Get(resource + "/codes").WithBearerToken(token).Build().Execute<ResponseModel>();
        }

        public IResponse<ResponseModelAccessTokenModel> PostClientCodes(SubmitCodeModel model, string token)
        {
            return Request.Post(resource + "/codes").AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModelAccessTokenModel>();
        }

        public IResponse<ResponseModelEncodedKeyModel> PostClientEncodedMainKey(AccessTokenModel model, string token)
        {
            return Request.Post(resource + "/keys/encodedmainkey").WithBearerToken(token).AddJsonBody(model)
                .Build().Execute<ResponseModelEncodedKeyModel>();
        }

        public IResponse<ResponseModelClientBalancesModel> GetClientBalancesBaseAsset(string baseAsset, string token)
        {
            return Request.Get(resource + $"/balances/{baseAsset}").WithBearerToken(token).Build().Execute<ResponseModelClientBalancesModel>();
        }

        public IResponse<ResponseModel> PostClientDialogOk(string token)
        {
            return Request.Post(resource + "/pushTxDialogOk").WithBearerToken(token).Build().Execute<ResponseModel>();
        }

        public IResponse<ResponseModel> DeleteClientKey(string key, string token)
        {
            return Request.Delete(resource + $"/dictionary/{key}").WithBearerToken(token).Build().Execute<ResponseModel>();
        }

        public IResponse<ResponseModelIKeyValue> GetClientKey(string key, string token)
        {
            return Request.Get(resource + $"/dictionary/{key}").WithBearerToken(token).Build().Execute<ResponseModelIKeyValue>();
        }

        public IResponse<ResponseModel> PostClientDictionary(KeyValue model, string token)
        {
            return Request.Post(resource + "/dictionary").AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModel>();
        }

        public IResponse<ResponseModel> PutClientDictionary(KeyValue model, string token)
        {
            return Request.Put(resource + "/dictionary").AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModel>();
        }

        public IResponse<ResponseModelIsUserFromUSModel> GetClientIsFromUs(string token)
        {
            return Request.Get(resource + "/properties/isUserFromUS").WithBearerToken(token).Build().Execute<ResponseModelIsUserFromUSModel>();
        }

        public IResponse<ResponseModel> PostClientIsFromUs(SetIsUserFromUSModel model, string token)
        {
            return Request.Post(resource + "/properties/isUserFromUS").AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModel>();
        }

        public IResponse<ResponseModelPendingActionsModel> GetClientPendingActions(string token)
        {
            return Request.Get(resource + "/pendingActions").WithBearerToken(token).Build().Execute<ResponseModelPendingActionsModel>();
        }
    }
}
