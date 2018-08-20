using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs;
using XUnitTestCommon.GlobalActions;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;
using XUnitTestCommon.Tests;

namespace BalancesData.Fixtures
{
    [TestFixture]
    public class BalancesTestDataFixture : BaseTest
    {
        private BalancesSettings _balancesSettings;
        private BlueApiSettings _blueApiSettings;
        public ApiConsumer Consumer;
        public ClientRegisterResponseDTO TestClient;


        [OneTimeSetUp]
        public void Initialize()
        {
            _balancesSettings = new ConfigBuilder().ReloadingManager.CurrentValue.AutomatedFunctionalTests.Balances;
            _blueApiSettings = new ConfigBuilder().ReloadingManager.CurrentValue.AutomatedFunctionalTests.BlueApi;
            Consumer = new ApiConsumer(_balancesSettings, null);

            prepareTestData().Wait();
        }

        private async Task prepareTestData()
        {
            ApiConsumer registerConsumer = new ApiConsumer(_blueApiSettings);
            this.TestClient = await registerConsumer.RegisterNewUser();

            await ClientAccounts.FillWalletWithAsset(TestClient.Account.Id, Constants.BALANCES_ASSET_ID, Constants.BALANCES_ASSET_AMOUNT);
        }
    }
}
