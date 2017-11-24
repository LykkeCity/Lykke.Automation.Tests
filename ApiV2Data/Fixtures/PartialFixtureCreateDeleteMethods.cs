using ApiV2Data.DTOs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;

namespace ApiV2Data.Fixtures
{
    public partial class ApiV2TestDataFixture : BaseTest
    {
        public async Task<WalletDTO> CreateTestWallet(bool isHFT = false)
        {
            string url = ApiPaths.WALLETS_BASE_PATH;
            if (isHFT)
                url += "/hft";

            WalletCreateDTO newWallet = new WalletCreateDTO()
            {
                Name = Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                Description = Guid.NewGuid().ToString() + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest
            };
            string createParam = JsonUtils.SerializeObject(newWallet);

            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.OK)
            {
                return null;
            }

            WalletDTO returnModel;

            if (isHFT)
            {
                returnModel = new WalletDTO();
                WalletCreateHFTDTO createdDTO = JsonUtils.DeserializeJson<WalletCreateHFTDTO>(response.ResponseJson);
                returnModel.Id = createdDTO.WalletId;
                returnModel.ApiKey = createdDTO.ApiKey;
                returnModel.Name = newWallet.Name;
                returnModel.Description = newWallet.Description;
            }
            else
            {
                returnModel = JsonUtils.DeserializeJson<WalletDTO>(response.ResponseJson);
            }

            AddOneTimeCleanupAction(async () => await DeleteTestWallet(returnModel.Id));

            return returnModel;
        }

        public async Task<bool> DeleteTestWallet(string id)
        {
            string url = ApiPaths.WALLETS_BASE_PATH + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }

        public async Task<OperationCreateReturnDTO> CreateTestOperation()
        {
            string newId = Guid.NewGuid().ToString().ToLower();
            string url = ApiPaths.OPERATIONS_TRANSFER_PATH + "/" +newId;

            OperationCreateDTO createDTO = new OperationCreateDTO()
            {
                Amount = Helpers.Random.Next(1, 10),
                AssetId = TestAssetId,
                SourceWalletId = TestWalletWithBalance,
                WalletId = TestWalletOperations.Id
            };
            string createParam = JsonUtils.SerializeObject(createDTO);


            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }
            string parsedResponse = JsonUtils.DeserializeJson<string>(response.ResponseJson);
            OperationCreateReturnDTO returnDTO = new OperationCreateReturnDTO(createDTO)
            {
                Id = parsedResponse
            };

            AddOneTimeCleanupAction(async () => await CancelTestOperation(returnDTO.Id));

            return returnDTO;
        }

        public async Task<bool> CancelTestOperation(string id)
        {
            string url = ApiPaths.OPERATIONS_CANCEL_PATH +  "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            if (response.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }
    }
}
