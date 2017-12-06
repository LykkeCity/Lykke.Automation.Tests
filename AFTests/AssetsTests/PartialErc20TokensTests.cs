using System;
using System.Net;
using System.Threading.Tasks;
using AssetsData.DTOs.Assets;
using NUnit.Framework;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using AssetsData;
using System.Linq;
using FluentAssertions;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest
    {
        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensGetAll")]
        public async Task GetAllErc20Tokens()
        {
            var url = ApiPaths.ERC20TOKENS_BASE_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            Erc20TokenItemsDto parsedResponse = JsonUtils.DeserializeJson<Erc20TokenItemsDto>(response.ResponseJson);
        }

        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensGetAllWithAssets")]
        public async Task GetAllErc20TokensWithAssets()
        {
            var url = $"{ApiPaths.ERC20TOKENS_BASE_PATH}/with-assets";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            Erc20TokenItemsDto parsedResponse = JsonUtils.DeserializeJson<Erc20TokenItemsDto>(response.ResponseJson);
        }

        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensGetByAddress")]
        public async Task GetErc20TokenByAddress()
        {
            string address = HttpUtility.UrlEncode(this.TestErcToken.Address);
            var url = $"{ApiPaths.ERC20TOKENS_BASE_PATH}/{address}";

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            Erc20TokenDto parsedResponse = JsonUtils.DeserializeJson<Erc20TokenDto>(response.ResponseJson);

            Assert.True(parsedResponse.Address == this.TestErcToken.Address);

        }

        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensGetBySpecification")]
        public async Task GetErc20TokensBySpecifications()
        {
            var url = $"{ApiPaths.ERC20TOKENS_BASE_PATH}/__specification";
            var body = new
            {
                Ids = Constants.ERC_TOKEN_ASSET_IDS
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            Erc20TokenItemsDto parsedResponse = JsonUtils.DeserializeJson<Erc20TokenItemsDto>(response.ResponseJson);

            foreach (Erc20TokenDto token in parsedResponse.Items)
            {
                Assert.True(Constants.ERC_TOKEN_ASSET_IDS.Contains(token.AssetId));
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensCreate")]
        public async Task CreateErc20Token()
        {
            var url = ApiPaths.ERC20TOKENS_BASE_PATH;
            var rndValue = Helpers.Random.Next(1000000);
            var body = new Erc20TokenDto
            {
                AssetId = Constants.ERC_TOKEN_ASSET_IDS[0],
                Address = $"0x+fake_{rndValue}",
                BlockHash = "fake",
                BlockTimestamp = rndValue,
                DeployerAddress = "fake",
                TokenDecimals = 1,
                TokenName = String.Empty,
                TokenSymbol = "fake",
                TokenTotalSupply = "1",
                TransactionHash = "fake"
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);

            Assert.True(response.Status == HttpStatusCode.OK);

            Erc20TokenDto checkDto = await GetTokenByAddress(body.Address);

            checkDto.ShouldBeEquivalentTo(body);
        }

        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensUpdate")]
        public async Task UpdateErc20Token()
        {
            //REMARK: It is expected for update token to create new one 
            //if the one we are trying to update does not exist
            var url = ApiPaths.ERC20TOKENS_BASE_PATH;
            var rndValue = Helpers.Random.Next(1000000);
            var body = new Erc20TokenDto
            {
                AssetId = Constants.ERC_TOKEN_ASSET_IDS[0],
                Address = this.TestErcToken.Address,
                BlockHash = "fake",
                BlockTimestamp = rndValue,
                DeployerAddress = "fake",
                TokenDecimals = 1,
                TokenName = String.Empty,
                TokenSymbol = "fake",
                TokenTotalSupply = "1",
                TransactionHash = "fake"
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.PUT);

            Assert.True(response.Status == HttpStatusCode.NoContent);

            Erc20TokenDto checkDto = await GetTokenByAddress(body.Address);

            checkDto.ShouldBeEquivalentTo(body);
        }

        [Ignore("Test will fail, cause when we try to add same asset second time it will throw an error")]
        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensCreateAsset")]
        public async Task CreateErc20TokenAsset()
        {
            var address = HttpUtility.UrlEncode($"0x+fake_0{Helpers.Random.Next(100000)}");
            var url = $"{ApiPaths.ERC20TOKENS_BASE_PATH}/{address}/create-asset";

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.PUT);

            Assert.True(response.Status == HttpStatusCode.Created);
            Assert.NotNull(response.ResponseJson);
        }

        private async Task<Erc20TokenDto> GetTokenByAddress(string address)
        {
            string addr = HttpUtility.UrlEncode(address);
            var url = $"{ApiPaths.ERC20TOKENS_BASE_PATH}/{addr}";

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            if (response.Status != HttpStatusCode.OK)
            {
                return null;
            }

            Erc20TokenDto parsedResponse = JsonUtils.DeserializeJson<Erc20TokenDto>(response.ResponseJson);

            return parsedResponse;
        }
    }
}