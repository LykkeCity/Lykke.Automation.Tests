﻿using ApiV2Data.Fixtures;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using ApiV2Data.DTOs;
using XUnitTestCommon.Utils;
using FluentAssertions;
using NUnit.Framework;
using XUnitTestData.Entities.ApiV2;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests
    {
        [Test]
        [Category("Smoke")]
        [Category("Operations")]
        [Category("OperationsGet")]
        public async Task GetOperationById()
        {
            string url = ApiPaths.OPERATIONS_BASE_PATH + "/" + this.TestOperation.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationDTO parsedResponse = JsonUtils.DeserializeJson<OperationDTO>(response.ResponseJson);
            OperationsEntity entity = await this.OperationsRepository.TryGetAsync(parsedResponse.Id) as OperationsEntity;

            OperationContext entityContext = JsonUtils.DeserializeJson<OperationContext>(entity.Context);

            entityContext.ShouldBeEquivalentTo(parsedResponse.Context);
            Assert.True(entity.StatusString == parsedResponse.Status);
            Assert.True(entity.TypeString == parsedResponse.Type);
            Assert.True(entity.ClientId.ToString().ToLower() == parsedResponse.ClientId);
            Assert.True(entity.Created == parsedResponse.Created);
        }

        [Test]
        [Category("Smoke")]
        [Category("Operations")]
        [Category("OperationsPost")]
        public async Task CreateOperation()
        {
            OperationCreateReturnDTO createdDTO = await this.CreateTestOperation();
            Assert.NotNull(createdDTO);
            OperationsEntity entity = await this.OperationsRepository.TryGetAsync(createdDTO.Id) as OperationsEntity;
            OperationsEntity createdEntity = await this.OperationsRepository.TryGetAsync("Created", createdDTO.Id) as OperationsEntity;
            Assert.NotNull(createdEntity);

            Assert.True(entity.StatusString == "Created");
            Assert.True(entity.TypeString == "Transfer");
        }

        [Test]
        [Category("Smoke")]
        [Category("Operations")]
        [Category("OperationsPost")]
        public async Task CancelOperation()
        {
            string url = ApiPaths.OPERATIONS_CANCEL_PATH + "/" + this.TestOperationCancel.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationsEntity entity = await this.OperationsRepository.TryGetAsync(this.TestOperationCancel.Id) as OperationsEntity;
            OperationsEntity canceledEntity = await this.OperationsRepository.TryGetAsync("Canceled", this.TestOperationCancel.Id) as OperationsEntity;
            Assert.NotNull(canceledEntity);

            Assert.True(entity.StatusString == "Canceled");
        }

        [Test]
        [Category("Smoke")]
        [Category("OperationDetails")]
        [Category("OperationDetailsPost")]
        public async Task CreateOperationDetails()
        {
            string url = ApiPaths.OPERATIONS_DETAILS_CREATE_PATH;
            OperationDetailsDTO createDTO = new OperationDetailsDTO()
            {
                TransactionId = this.TestOperationCreateDetails.Id,
                Comment = Guid.NewGuid().ToString() + GlobalConstants.AutoTest
            };
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationDetailsEntity entity = await this.OperationDetailsRepository.TryGetAsync(
                d => d.PartitionKey == this.TestClientId && d.TransactionId == this.TestOperationCreateDetails.Id) as OperationDetailsEntity;
            Assert.NotNull(entity);

            Assert.True(entity.Comment == createDTO.Comment);
        }

        [Test]
        [Category("Smoke")]
        [Category("OperationDetails")]
        [Category("OperationDetailsPost")]
        public async Task RegisterOperationDetails()
        {
            string url = ApiPaths.OPERATIONS_DETAILS_REGISTER_PATH;
            OperationDetailsDTO createDTO = new OperationDetailsDTO()
            {
                TransactionId = this.TestOperationRegisterDetails.Id,
                Comment = Guid.NewGuid().ToString() + GlobalConstants.AutoTest
            };
            string createParam = JsonUtils.SerializeObject(createDTO);

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, createParam, Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            OperationDetailsReturnDTO parsedResponse = JsonUtils.DeserializeJson<OperationDetailsReturnDTO>(response.ResponseJson);

            OperationDetailsEntity entity = await this.OperationDetailsRepository.TryGetAsync(this.TestClientId, parsedResponse.Id) as OperationDetailsEntity;
            Assert.NotNull(entity);

            Assert.True(entity.TransactionId == createDTO.TransactionId);
            Assert.True(entity.Comment == createDTO.Comment);
        }
    }
}
