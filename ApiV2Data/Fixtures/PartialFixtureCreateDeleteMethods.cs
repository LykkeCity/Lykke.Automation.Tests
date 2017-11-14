using ApiV2Data.DTOs;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Utils;

namespace ApiV2Data.Fixtures
{
    public partial class ApiV2TestDataFixture : IDisposable
    {
        public async Task<WalletDTO> CreateTestWallet(bool isHFT = false)
        {
            string url = ApiEndpointNames["Wallets"];
            if (isHFT)
                url += "/hft";

            WalletCreateDTO newWallet = new WalletCreateDTO()
            {
                Name = Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest",
                Description = Guid.NewGuid().ToString() + Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest"
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

            _walletsToDelete.Add(returnModel.Id);

            return returnModel;
        }

        public async Task<bool> DeleteTestWallet(string id)
        {
            string url = ApiEndpointNames["Wallets"] + "/" + id;
            var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            if (response.Status != HttpStatusCode.OK)
            {
                return false;
            }
            return true;
        }
    }
}
