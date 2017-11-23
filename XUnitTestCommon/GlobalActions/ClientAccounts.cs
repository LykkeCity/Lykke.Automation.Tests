using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.Consumers;

namespace XUnitTestCommon.GlobalActions
{
    public class ClientAccounts
    {

        public static async Task<bool> DeleteClientAccount(string clientId)
        {
            ApiConsumer consumer = new ApiConsumer(ApiPaths.CLIENT_ACCOUNT_SERVICE_PREFIX, ApiPaths.CLIENT_ACCOUNT_SERVICE_BASEURL, false);

            string url = ApiPaths.CLIENT_ACCOUNT_PATH + "/" + clientId;
            var deleteResponse = await consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }
    }
}
