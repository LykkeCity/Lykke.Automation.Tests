﻿using AlgoStoreData.DTOs;
using ApiV2Data.DTOs;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities;

namespace AlgoStoreData.Fixtures
{
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        private string endpoint = "https://apiv2-dev.lykkex.net";
        private string walletPath = ApiPaths.WALLETS_BASE_PATH;

        public async Task<WalletDTO> GetExistingWallet()
        {
            var response = await Consumer.ExecuteRequestCustromEndpoint($"{endpoint}{walletPath}", Helpers.EmptyDictionary, null, Method.GET);
            List<WalletDTO> walletDTOs = JsonUtils.DeserializeJson<List<WalletDTO>>(response.ResponseJson);

            walletDTOs.RemoveAll(x => x.Name == "Trading");

            Random rnd = new Random();
            int rndIdx = rnd.Next(walletDTOs.Count);

            var walletId = walletDTOs[rndIdx].Id;

            // TODO: Make traded assets random
            // Add some funds to the wallet
            await AddAssetFundsToWallet(walletId, "EUR", 100);
            await AddAssetFundsToWallet(walletId, "USD", 100);
            await AddAssetFundsToWallet(walletId, "BTC", 10);

            return walletDTOs[rndIdx];
        }

        public async Task<WalletDTO> CreateTestWallet()
        {
            string createWalletPath = $"{endpoint}{walletPath}/hft";

            string utcTimestamp = DateTime.UtcNow.ToString("s");
            WalletCreateDTO newWallet = new WalletCreateDTO
            {
                Name = $"{GlobalConstants.AutoTest}_Wallet_{utcTimestamp}",
                Description = $"{GlobalConstants.AutoTest}_Wallet_{utcTimestamp} - Description"
            };

            string createParam = JsonUtils.SerializeObject(newWallet);    

            var response = await Consumer.ExecuteRequestCustromEndpoint(endpoint + createWalletPath, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.OK)
            {
                return null;
            }

            WalletDTO returnModel = new WalletDTO();

            WalletCreateHFTDTO createdDTO = JsonUtils.DeserializeJson<WalletCreateHFTDTO>(response.ResponseJson);
            returnModel.Id = createdDTO.WalletId;
            returnModel.ApiKey = createdDTO.ApiKey;
            returnModel.Name = newWallet.Name;
            returnModel.Description = newWallet.Description;

            // TODO: Make traded assets random
            // Add some funds to the wallet
            await AddAssetFundsToWallet(createdDTO.WalletId, "EUR", 100);
            await AddAssetFundsToWallet(createdDTO.WalletId, "USD", 100);
            await AddAssetFundsToWallet(createdDTO.WalletId, "BTC", 10);

            AddOneTimeCleanupAction(async () => await DeleteTestWallet(returnModel.Id));
            return returnModel;
        }

        public async Task<bool> DeleteTestWallet(string id)
        {
            string deleteWalletPath = $"{ApiPaths.WALLETS_BASE_PATH}/{id}";
            var response = await Consumer.ExecuteRequestCustromEndpoint(endpoint + deleteWalletPath, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

        public async Task<double> GetWalletBalanceByAssetId(string walletId, string assetId)
        {
            string path = $"/api/wallets/{walletId}/balances/{assetId}";
            var response = await Consumer.ExecuteRequestCustromEndpoint(endpoint + path, Helpers.EmptyDictionary, null, Method.GET);
            double balance;
            switch(response.Status)
            {
                case HttpStatusCode.OK:
                    BalanceDTO walletBalanceDTO = JsonUtils.DeserializeJson<BalanceDTO>(response.ResponseJson);
                    balance = walletBalanceDTO.Balance;
                    break;
                case HttpStatusCode.NotFound: // Service returns not found if the balance is 0
                    balance = 0;
                    break;
                default:
                    throw new Exception($"Cannot get balance of Asset: {assetId} for WalletId: {walletId}. Response: {response.ResponseJson}");
            }
            

            return balance;
        }

        public async Task AddAssetFundsToWallet(string walletId, string assetId, double amount)
        {
            var walletBalance = await GetWalletBalanceByAssetId(walletId, assetId);
            if (walletBalance < 10)
            {
                var exchangeOperationsURL = "http://exchange-operations.lykke-service.svc.cluster.local";
                string path = "/api/ExchangeOperations/ManualCashIn";
                String clientId = await GetClientIdByEmail(User.AuthUser.Email);
                ManualCashInDTO manualCashInDTO = new ManualCashInDTO(clientId, walletId, assetId, amount);
                var requestBody = JsonUtils.SerializeObject(manualCashInDTO);
                var response = await Consumer.ExecuteRequestCustromEndpoint($"{exchangeOperationsURL}{path}", Helpers.EmptyDictionary, requestBody, Method.POST);
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK), $"Cannot add fund for Asset: {assetId} for WalletId: {walletId}. Response: {response.ResponseJson}");

                // Wait up to twenty seconds after adding funds throught the API in order for the funds to appear in the wallet
                int retries = 10;
                while (await GetWalletBalanceByAssetId(walletId, assetId) == 0 && retries > 0)
                {
                    Wait.ForPredefinedTime(2000);
                    retries--;
                }
            }
        }

        public async Task<String> GetClientIdByEmail(string email)
        {
            var clientAccountUrl = "http://client-account.lykke-service.svc.cluster.local";
            var getClientDetailsPath = $"/api/ClientAccountInformation/getClientsByEmail/{email}";
            var response = await Consumer.ExecuteRequestCustromEndpoint($"{clientAccountUrl}{getClientDetailsPath}", Helpers.EmptyDictionary, null, Method.GET);
            List<ClientAccount> clients = JsonUtils.DeserializeJson<List<ClientAccount>>(response.ResponseJson);

            if (clients.Count > 0)
            {
                return clients.First().Id;
            } else
            {
                throw new Exception($"No client found for user with email: ${email}");
            }
        }
    }
}