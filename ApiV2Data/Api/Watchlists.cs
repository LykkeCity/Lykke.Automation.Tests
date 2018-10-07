using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Watchlists : ApiBase
    {
        public IResponse<List<WatchList>> GetWatchlists(string token) =>
            Request.Get("/watchlists").WithBearerToken(token).Build().Execute<List<WatchList>>();

        public IResponse<WatchListModel> PostWatchLists(WatchListCreateModel model, string token) =>
            Request.Post("/watchlists").WithBearerToken(token).AddJsonBody(model).Build().Execute<WatchListModel>();

        public IResponse DeleteWatchlists(string id, string token) =>
            Request.Delete($"/watchlists/{id}").WithBearerToken(token).Build().Execute();

        public IResponse<WatchListModel> GetWatchlistsById(string id, string token) =>
            Request.Get($"/watchlists/{id}").WithBearerToken(token).Build().Execute<WatchListModel>();
        
        public IResponse<WatchListModel> PutWatchlistsById(WatchListUpdateModel model, string id, string token) => 
            Request.Put($"/watchlists/{id}").AddJsonBody(model).WithBearerToken(token).Build().Execute<WatchListModel>();
    }
}
