using ApiV2Data.Fixtures;
using RestSharp;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTestCommon;
using ApiV2Data.DTOs;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.ApiV2;
using FluentAssertions;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Operations")]
        [Trait("Category", "OperationsGet")]
        public async void GetOperationById()
        {
            string url = ApiPaths.OPERATIONS_BASE_PATH + "/" + _fixture.TestOperation.Id;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationDTO parsedResponse = JsonUtils.DeserializeJson<OperationDTO>(response.ResponseJson);
            OperationsEntity entity = await _fixture.OperationsRepository.TryGetAsync(parsedResponse.Id) as OperationsEntity;

            OperationContext entityContext = JsonUtils.DeserializeJson<OperationContext>(entity.Context);

            entityContext.ShouldBeEquivalentTo(parsedResponse.Context);
            Assert.True(entity.StatusString == parsedResponse.Status);
            Assert.True(entity.TypeString == parsedResponse.Type);
            Assert.True(entity.ClientId.ToString().ToLower() == parsedResponse.ClientId);
            Assert.True(entity.Created == parsedResponse.Created);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Operations")]
        [Trait("Category", "OperationsPost")]
        public async void CreateOperation()
        {
            OperationCreateReturnDTO createdDTO = await _fixture.CreateTestOperation();
            Assert.NotNull(createdDTO);
            OperationsEntity entity = await _fixture.OperationsRepository.TryGetAsync(createdDTO.Id) as OperationsEntity;
            OperationsEntity createdEntity = await _fixture.OperationsRepository.TryGetAsync("Created", createdDTO.Id) as OperationsEntity;
            Assert.NotNull(createdEntity);

            Assert.True(entity.StatusString == "Created");
            Assert.True(entity.TypeString == "Transfer");
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Operations")]
        [Trait("Category", "OperationsPost")]
        public async void CancelOperation()
        {
            string url = ApiPaths.OPERATIONS_CANCEL_PATH + "/" + _fixture.TestOperationCancel.Id;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationsEntity entity = await _fixture.OperationsRepository.TryGetAsync(_fixture.TestOperationCancel.Id) as OperationsEntity;
            OperationsEntity canceledEntity = await _fixture.OperationsRepository.TryGetAsync("Canceled", _fixture.TestOperationCancel.Id) as OperationsEntity;
            Assert.NotNull(canceledEntity);

            Assert.True(entity.StatusString == "Canceled");
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "OperationDetails")]
        [Trait("Category", "OperationDetailsPost")]
        public async void CreateOperationDetails()
        {
            string url = ApiPaths.OPERATIONS_DETAILS_CREATE_PATH;
            OperationDetailsDTO createDTO = new OperationDetailsDTO()
            {
                TransactionId = _fixture.TestOperationCreateDetails.Id,
                Comment = Guid.NewGuid().ToString() + GlobalConstants.AutoTest
            };
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationDetailsEntity entity = await _fixture.OperationDetailsRepository.TryGetByTransactionId(_fixture.TestClientId, _fixture.TestOperationCreateDetails.Id) as OperationDetailsEntity;
            Assert.NotNull(entity);

            Assert.True(entity.Comment == createDTO.Comment);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "OperationDetails")]
        [Trait("Category", "OperationDetailsPost")]
        public async void RegisterOperationDetails()
        {
            string url = ApiPaths.OPERATIONS_DETAILS_REGISTER_PATH;
            OperationDetailsDTO createDTO = new OperationDetailsDTO()
            {
                TransactionId = _fixture.TestOperationRegisterDetails.Id,
                Comment = Guid.NewGuid().ToString() + GlobalConstants.AutoTest;
            };
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationDetailsReturnDTO parsedResponse = JsonUtils.DeserializeJson<OperationDetailsReturnDTO>(response.ResponseJson);

            OperationDetailsEntity entity = await _fixture.OperationDetailsRepository.TryGetAsync(_fixture.TestClientId, parsedResponse.Id) as OperationDetailsEntity;
            Assert.NotNull(entity);

            Assert.True(entity.TransactionId == createDTO.TransactionId);
            Assert.True(entity.Comment == createDTO.Comment);
        }
    }
}
