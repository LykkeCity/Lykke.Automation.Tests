﻿using System;
using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities.BlueApi;
using XUnitTestData.Entities.ApiV2;

namespace BlueApiData.Fixtures
{
    public partial class BlueApiTestDataFixture
    {
        public async Task<PledgeDTO> CreateTestPledge(string clientId, string consumerIndex = null)
        {
            var consumer = String.IsNullOrEmpty(consumerIndex) ? Consumer : PledgeApiConsumers[consumerIndex];

            var url = ApiPaths.PLEDGES_BASE_PATH;
            var newPledge = new CreatePledgeDTO
            {
                CO2Footprint = Helpers.Random.Next(100, 100000),
                ClimatePositiveValue = Helpers.Random.Next(100, 100000)
            };

            var createParam = JsonUtils.SerializeObject(newPledge);
            var response = await consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            if (response.Status != HttpStatusCode.Created)
            {
                return null;
            }

            var pledge = await PledgeRepository.TryGetAsync(
                p =>
                {
                    return p.PartitionKey == PledgeEntity.GeneratePartitionKey() && p.ClientId == clientId;
                });

            var returnDto = Mapper.Map<PledgeDTO>(pledge);

            return returnDto;
        }

        public async Task<bool> DeleteTestPledge(string consumerIndex = null)
        {
            var consumer = String.IsNullOrEmpty(consumerIndex) ? Consumer : PledgeApiConsumers[consumerIndex];

            var deletePledgeUrl = ApiPaths.PLEDGES_BASE_PATH;
            var deleteResponse = await consumer.ExecuteRequest(deletePledgeUrl, Helpers.EmptyDictionary, null, Method.DELETE);

            if (deleteResponse.Status != HttpStatusCode.NoContent)
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetClientIdByEmail(string email)
        {
            PersonalDataEntity entity = await PersonalDataRepository.TryGetAsync(pd => pd.PartitionKey == PersonalDataEntity.GeneratePartitionKey() && pd.Email == email) as PersonalDataEntity;
            if (entity != null)
                return entity.RowKey;
            return null;
        }
    }
}
