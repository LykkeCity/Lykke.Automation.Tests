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
            string url = fixture.ApiEndpointNames["Operations"] + "/" + fixture.TestOperation.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationDTO parsedResponse = JsonUtils.DeserializeJson<OperationDTO>(response.ResponseJson);
            OperationsEntity entity = await fixture.OperationsRepository.TryGetAsync(parsedResponse.Id) as OperationsEntity;

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
            OperationCreateReturnDTO createdDTO = await fixture.CreateTestOperation();
            Assert.NotNull(createdDTO);
            OperationsEntity entity = await fixture.OperationsRepository.TryGetAsync(createdDTO.Id) as OperationsEntity;
            OperationsEntity createdEntity = await fixture.OperationsRepository.TryGetAsync("Created", createdDTO.Id) as OperationsEntity;
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
            string url = fixture.ApiEndpointNames["Operations"] + "/cancel/" + fixture.TestOperationCancel.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationsEntity entity = await fixture.OperationsRepository.TryGetAsync(fixture.TestOperationCancel.Id) as OperationsEntity;
            OperationsEntity canceledEntity = await fixture.OperationsRepository.TryGetAsync("Canceled", fixture.TestOperationCancel.Id) as OperationsEntity;
            Assert.NotNull(canceledEntity);

            Assert.True(entity.StatusString == "Canceled");
        }
    }
}
