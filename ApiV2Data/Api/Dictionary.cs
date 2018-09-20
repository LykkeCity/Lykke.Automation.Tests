using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Dictionary : ApiBase
    {
        public IResponse<DataModel> GetDictionaryKey(string key, string token)
        {
            return Request.Get($"/dictionary/{key}").WithBearerToken(token).Build().Execute<DataModel>();
        }

        public IResponse DeleteDictionarykey(string key, string token)
        {
            return Request.Delete($"/dictionary/{key}").WithBearerToken(token).Build().Execute();
        }

        public IResponse PostDictionaryKey(DataModel model, string key, string token)
        {
            return Request.Post($"/dictionary/{key}").AddJsonBody(model).WithBearerToken(token).Build().Execute();
        }
    }
}
