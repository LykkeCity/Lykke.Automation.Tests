using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

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
        }

        [Test]
        [Category("Smoke")]
        [Category("Erc20Tokens")]
        [Category("Erc20TokensGetByAddress")]
        public async Task GetErc20TokenByAddress()
        {
            var url = $"{ApiPaths.ERC20TOKENS_BASE_PATH}/address";
            var body = new
            {
                address = "0x0c098d65f60ea5cfb27b7efe36b7de8db7622a7e" //TODO: Check if we should put this in constants
            };
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);
        }
    }
}