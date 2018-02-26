using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class WatchLists : ApiBase
    {
        public IResponse<ResponseModelIEnumerableWatchList> Get(string token) =>
            Request.Get("/WatchLists").WithBearerToken(token).Build().Execute<ResponseModelIEnumerableWatchList>();

        public IResponse<ResponseModelWatchList> Post(CustomWatchListCreateModel customWatchListCreate, string token) =>
            Request.Post("/WatchLists").WithBearerToken(token).
            AddJsonBody(customWatchListCreate).Build().Execute<ResponseModelWatchList>();

        public IResponse<ResponseModel> DeleteById(string id, string token) =>
            Request.Delete($"/WatchLists/{id}").WithBearerToken(token).Build().Execute<ResponseModel>();

        public IResponse<ResponseModelWatchList> GetById(string id, string token) =>
            Request.Get($"/WatchLists/{id}").WithBearerToken(token).Build().Execute<ResponseModelWatchList>();

        public IResponse<ResponseModelWatchList> PutById(string id, CustomWatchListUpdateModel customWatchListUpdate, string token) =>
            Request.Put($"/WatchLists/{id}").WithBearerToken(token)
            .AddJsonBody(customWatchListUpdate).Build().Execute<ResponseModelWatchList>();
    }
}
