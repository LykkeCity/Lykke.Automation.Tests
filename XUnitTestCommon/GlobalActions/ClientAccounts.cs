using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;

namespace XUnitTestCommon.GlobalActions
{
    public static class ClientAccounts
    {

        private static ConfigBuilder MeConfig;
        private static MatchingEngineConsumer MEConsumer;
        private static MatchingEngineSettings MESettings;

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

        public static async Task FillWalletWithAsset(string walletId, string assetId, double amount)
        {
            if (MEConsumer == null)
            {
                MeConfig = new ConfigBuilder();
                MESettings = MeConfig.ReloadingManager.CurrentValue.AutomatedFunctionalTests.MatchingEngine;
                MEConsumer = new MatchingEngineConsumer(MESettings.BaseUrl, Int32.Parse(MESettings.Port));

                Thread.Sleep(500);
            }

            await MEConsumer.Client.UpdateBalanceAsync(Guid.NewGuid().ToString(), walletId, assetId, amount);
        }
    }
}
