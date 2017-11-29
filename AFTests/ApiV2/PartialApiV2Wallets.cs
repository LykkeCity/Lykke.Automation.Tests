using ApiV2Data.Fixtures;
using RestSharp;
using System.Net;
using System.Collections.Generic;
using XUnitTestCommon;
using ApiV2Data.DTOs;
using XUnitTestCommon.Utils;
using FluentAssertions;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using XUnitTestData.Entities;
using XUnitTestData.Entities.ApiV2;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests
    {
        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetAllWallets()
        {
            string url = ApiPaths.WALLETS_BASE_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletDTO>>(response.ResponseJson);

            foreach (WalletEntity entity in this.AllWalletsFromDb)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(w => w.Id == entity.Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetSingleWallets()
        {
            string url = ApiPaths.WALLETS_BASE_PATH + "/" + this.TestWallet.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WalletDTO parsedResponse = JsonUtils.DeserializeJson<WalletDTO>(response.ResponseJson);

            this.TestWallet.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetAllWalletBalances()
        {
            string url = ApiPaths.WALLETS_BALANCES_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletBalanceDTO>>(response.ResponseJson);

            foreach (WalletBalanceDTO wbDTO in parsedResponse)
            {
                WalletEntity walletEntity = this.AllWalletsFromDb.Where(w => w.Id == wbDTO.Id).FirstOrDefault();
                if (walletEntity != null)
                {
                    Assert.True(walletEntity.Name == wbDTO.Name);
                    Assert.True(walletEntity.Type == wbDTO.Type);
                    Assert.True(walletEntity.Description == wbDTO.Description);
                }


                AccountEntity accountEntity = await this.AccountRepository.TryGetAsync(wbDTO.Id) as AccountEntity;

                if (accountEntity != null)
                {
                    foreach (BalanceDTO balanceDTO in accountEntity.BalancesParsed)
                    {
                        var parsedBalance = wbDTO.Balances.Where(b => b.AssetId == balanceDTO.Asset).FirstOrDefault();
                        Assert.NotNull(parsedBalance);
                        Assert.True(balanceDTO.Balance == parsedBalance.Balance);
                        Assert.True(balanceDTO.Reserved == parsedBalance.Reserved);
                    }
                }
                else
                {
                    Assert.True(wbDTO.Balances.Count == 0);
                }
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetBalancesByWalletId()
        {
            string url = ApiPaths.WALLETS_BASE_PATH + "/" + this.TestWallet.Id + "/balances";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WBalanceDTO>>(response.ResponseJson);

            foreach (BalanceDTO balanceDTO in this.TestWalletAccount.BalancesParsed)
            {
                var parsedBalance = parsedResponse.Where(b => b.AssetId == balanceDTO.Asset).FirstOrDefault();
                Assert.NotNull(parsedBalance);
                Assert.True(balanceDTO.Balance == parsedBalance.Balance);
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetWalletBalancesByAssetId()
        {
            string url = ApiPaths.WALLETS_BALANCES_PATH + "/" + this.TestAssetId;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletSingleBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletSingleBalanceDTO>>(response.ResponseJson);

            foreach (WalletSingleBalanceDTO wbDTO in parsedResponse)
            {
                WalletEntity walletEntity = this.AllWalletsFromDb.Where(w => w.Id == wbDTO.Id).FirstOrDefault();
                if (walletEntity != null)
                {
                    Assert.True(walletEntity.Name == wbDTO.Name);
                    Assert.True(walletEntity.Type == wbDTO.Type);
                    Assert.True(walletEntity.Description == wbDTO.Description);
                }


                AccountEntity accountEntity = await this.AccountRepository.TryGetAsync(wbDTO.Id) as AccountEntity;

                if (accountEntity != null)
                {
                    BalanceDTO balanceDTO = accountEntity.BalancesParsed.Where(b => b.Asset == this.TestAssetId).FirstOrDefault();
                    Assert.True(balanceDTO.Balance == wbDTO.Balances.Balance);
                    Assert.True(balanceDTO.Reserved == wbDTO.Balances.Reserved);
                }
                else
                {
                    if (wbDTO.Balances != null)
                    {
                        Assert.True(wbDTO.Balances.Balance == 0);
                    }
                }
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetWalletBalanceByWalletAndAssetId()
        {
            string url = ApiPaths.TWITTER_BASE_PATH + "/" + this.TestWallet.Id + "/balances/" + this.TestAssetId;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WBalanceDTO parsedResponse = JsonUtils.DeserializeJson<WBalanceDTO>(response.ResponseJson);

            BalanceDTO accountBalance = this.TestWalletAccount.BalancesParsed.Where(b => b.Asset == this.TestAssetId).FirstOrDefault();

            Assert.NotNull(accountBalance);
            Assert.True(parsedResponse.Balance == accountBalance.Balance);
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetWalletTradeBalances()
        {
            string url = ApiPaths.WALLETS_TRADING_BALANCES_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WBalanceDTO>>(response.ResponseJson);

            AccountEntity entity = await this.AccountRepository.TryGetAsync(this.TestClientId) as AccountEntity;
            Assert.NotNull(entity);

            foreach (BalanceDTO entityBalance in entity.BalancesParsed)
            {
                WBalanceDTO responseBalance = parsedResponse.Where(b => b.AssetId == entityBalance.Asset).FirstOrDefault();
                Assert.NotNull(responseBalance);
                Assert.True(entityBalance.Balance == responseBalance.Balance);
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async Task GetWalletTradeBalanceByAssetId()
        {
            string url = ApiPaths.WALLETS_TRADING_BALANCES_PATH + "/" + this.TestAssetId;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WBalanceDTO parsedResponse = JsonUtils.DeserializeJson<WBalanceDTO>(response.ResponseJson);

            AccountEntity entity = await this.AccountRepository.TryGetAsync(this.TestClientId) as AccountEntity;
            Assert.NotNull(entity);
            BalanceDTO entityBalance = entity.BalancesParsed.Where(b => b.Asset == this.TestAssetId).FirstOrDefault();
            Assert.NotNull(entityBalance);

            Assert.True(entityBalance.Balance == parsedResponse.Balance);
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsPost")]
        public async Task CreateWallet()
        {
            WalletDTO createdWallet = await this.CreateTestWallet();
            Assert.NotNull(createdWallet);

            WalletEntity entity = await this.WalletRepository.TryGetAsync(createdWallet.Id) as WalletEntity;
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(createdWallet, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsPost")]
        public async Task CreateHFTWallet()
        {
            WalletDTO createdWallet = await this.CreateTestWallet(true);
            Assert.NotNull(createdWallet);

            WalletEntity entity = await this.WalletRepository.TryGetAsync(createdWallet.Id) as WalletEntity;
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(createdWallet, o => o
            .ExcludingMissingMembers()
            .Excluding(w => w.Type));
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsDelete")]
        public async Task DeleteWallet()
        {
            string url = ApiPaths.WALLETS_BASE_PATH + "/" + this.TestWalletDelete.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            Assert.True(response.Status == HttpStatusCode.OK);

            WalletEntity entity = await this.WalletRepository.TryGetAsync(this.TestWalletDelete.Id) as WalletEntity;
            Assert.True(entity.State == "deleted");
        }

        [Test]
        [Category("Smoke")]
        [Category("WalletsHFT")]
        [Category("WalletsHFTPut")]
        public async Task RegenerateApiKey()
        {
            string url = ApiPaths.HFT_BASE_PATH + "/" + this.TestWalletRegenerateKey.Id + "/regenerateKey";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.OK);

            WalletCreateHFTDTO parsedResponse = JsonUtils.DeserializeJson<WalletCreateHFTDTO>(response.ResponseJson);
            Assert.True(this.TestWalletRegenerateKey.ApiKey != parsedResponse.ApiKey);

            string checkUrl = ApiPaths.WALLETS_BASE_PATH + "/" + this.TestWalletRegenerateKey.Id;
            var checkResponse = await this.Consumer.ExecuteRequest(checkUrl, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(checkResponse.Status == HttpStatusCode.OK);
            WalletDTO checkParsedResponse = JsonUtils.DeserializeJson<WalletDTO>(checkResponse.ResponseJson);

            Assert.True(checkParsedResponse.ApiKey == parsedResponse.ApiKey);

        }
    }
}
