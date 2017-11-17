using ApiV2Data.Fixtures;
using RestSharp;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon;
using ApiV2Data.DTOs;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.ApiV2;
using FluentAssertions;
using System.Linq;
using XUnitTestData.Repositories;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests : ApiV2TestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async void GetAllWallets()
        {
            string url = fixture.ApiEndpointNames["Wallets"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletDTO>>(response.ResponseJson);

            foreach (WalletEntity entity in fixture.AllWalletsFromDB)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(w => w.Id == entity.Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async void GetSingleWallets()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/" + fixture.TestWallet.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WalletDTO parsedResponse = JsonUtils.DeserializeJson<WalletDTO>(response.ResponseJson);

            fixture.TestWallet.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async void GetAllWalletBalances()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/balances";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletBalanceDTO>>(response.ResponseJson);

            foreach (WalletBalanceDTO wbDTO in parsedResponse)
            {
                WalletEntity walletEntity = fixture.AllWalletsFromDB.Where(w => w.Id == wbDTO.Id).FirstOrDefault();
                if (walletEntity != null)
                {
                    Assert.True(walletEntity.Name == wbDTO.Name);
                    Assert.True(walletEntity.Type == wbDTO.Type);
                    Assert.True(walletEntity.Description == wbDTO.Description);
                }


                AccountEntity accountEntity = await fixture.AccountManager.TryGetAsync(wbDTO.Id) as AccountEntity;

                if (accountEntity != null)
                {
                    foreach (BalanceDTO balanceDTO in accountEntity.BalancesParsed)
                    {
                        var parsedBalance = wbDTO.Balances.Where(b => b.AssetId == balanceDTO.Asset).FirstOrDefault();
                        Assert.NotNull(parsedBalance);
                        Assert.True(balanceDTO.Balance == parsedBalance.Balance);
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
        public async void GetBalancesByWalletId()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/" + fixture.TestWallet.Id + "/balances";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WBalanceDTO>>(response.ResponseJson);

            foreach (BalanceDTO balanceDTO in fixture.TestWalletAccount.BalancesParsed)
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
        public async void GetWalletBalancesByAssetId()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/balances/" + fixture.TestWalletAssetId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletSingleBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletSingleBalanceDTO>>(response.ResponseJson);

            foreach (WalletSingleBalanceDTO wbDTO in parsedResponse)
            {
                WalletEntity walletEntity = fixture.AllWalletsFromDB.Where(w => w.Id == wbDTO.Id).FirstOrDefault();
                if (walletEntity != null)
                {
                    Assert.True(walletEntity.Name == wbDTO.Name);
                    Assert.True(walletEntity.Type == wbDTO.Type);
                    Assert.True(walletEntity.Description == wbDTO.Description);
                }


                AccountEntity accountEntity = await fixture.AccountManager.TryGetAsync(wbDTO.Id) as AccountEntity;

                if (accountEntity != null)
                {
                    BalanceDTO balanceDTO = accountEntity.BalancesParsed.Where(b => b.Asset == fixture.TestWalletAssetId).FirstOrDefault();
                    Assert.True(balanceDTO.Balance == wbDTO.Balances.Balance);
                }
                else
                {
                    Assert.Null(wbDTO.Balances);
                }
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async void GetWalletBalanceByWalletAndAssetId()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/" + fixture.TestWallet.Id + "/balances/" + fixture.TestWalletAssetId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WBalanceDTO parsedResponse = JsonUtils.DeserializeJson<WBalanceDTO>(response.ResponseJson);

            BalanceDTO accountBalance = fixture.TestWalletAccount.BalancesParsed.Where(b => b.Asset == fixture.TestWalletAssetId).FirstOrDefault();

            Assert.NotNull(accountBalance);
            Assert.True(parsedResponse.Balance == accountBalance.Balance);
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsGet")]
        public async void GetWalletTradeBalances()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/trading/balances";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WBalanceDTO>>(response.ResponseJson);

            AccountEntity entity = await fixture.AccountManager.TryGetAsync(fixture.TestClientId) as AccountEntity;
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
        public async void GetWalletTradeBalanceByAssetId()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/trading/balances/" + fixture.TestWalletAssetId;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WBalanceDTO parsedResponse = JsonUtils.DeserializeJson<WBalanceDTO>(response.ResponseJson);

            AccountEntity entity = await fixture.AccountManager.TryGetAsync(fixture.TestClientId) as AccountEntity;
            Assert.NotNull(entity);
            BalanceDTO entityBalance = entity.BalancesParsed.Where(b => b.Asset == fixture.TestWalletAssetId).FirstOrDefault();
            Assert.NotNull(entityBalance);

            Assert.True(entityBalance.Balance == parsedResponse.Balance);
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsPost")]
        public async void CreateWallet()
        {
            WalletDTO createdWallet = await fixture.CreateTestWallet();
            Assert.NotNull(createdWallet);

            WalletEntity entity = await fixture.WalletRepository.TryGetAsync(createdWallet.Id) as WalletEntity;
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(createdWallet, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsPost")]
        public async void CreateHFTWallet()
        {
            WalletDTO createdWallet = await fixture.CreateTestWallet(true);
            Assert.NotNull(createdWallet);

            WalletEntity entity = await fixture.WalletRepository.TryGetAsync(createdWallet.Id) as WalletEntity;
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(createdWallet, o => o
            .ExcludingMissingMembers()
            .Excluding(w => w.Type));
        }

        [Test]
        [Category("Smoke")]
        [Category("Wallets")]
        [Category("WalletsDelete")]
        public async void DeleteWallet()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/" + fixture.TestWalletDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            Assert.True(response.Status == HttpStatusCode.OK);

            WalletEntity entity = await fixture.WalletRepository.TryGetAsync(fixture.TestWalletDelete.Id) as WalletEntity;
            Assert.True(entity.State == "deleted");
        }
    }
}
