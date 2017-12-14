using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs;
using XUnitTestCommon.GlobalActions;
using XUnitTestCommon.Tests;

namespace BalancesData.Fixtures
{
    [TestFixture]
    public class BalancesTestDataFixture : BaseTest
    {
        private ConfigBuilder _configBuilder;
        public ApiConsumer Consumer;
        public ClientRegisterResponseDTO TestClient;


        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("Balances");
            Consumer = new ApiConsumer(_configBuilder, null);

            prepareTestData().Wait();
        }

        private async Task prepareTestData()
        {
            ApiConsumer registerConsumer = new ApiConsumer(new ConfigBuilder("BlueApi"));
            this.TestClient = await registerConsumer.RegisterNewUser();

            await ClientAccounts.FillWalletWithAsset(TestClient.Account.Id, Constants.BALANCES_ASSET_ID, Constants.BALANCES_ASSET_AMOUNT);
        }
    }
}
