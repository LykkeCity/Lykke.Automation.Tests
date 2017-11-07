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
using System.Linq;
using XUnitTestData.Repositories;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
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

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetSingleWallets()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/" + fixture.TestWallet.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WalletDTO parsedResponse = JsonUtils.DeserializeJson<WalletDTO>(response.ResponseJson);

            fixture.TestWallet.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
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

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetBalancesByWalletId()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/" + fixture.TestWallet.Id + "/balances";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletBalanceDTO.BalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletBalanceDTO.BalanceDTO>>(response.ResponseJson);

            AccountEntity accountEntity = await fixture.AccountManager.TryGetAsync(fixture.TestWallet.Id) as AccountEntity;

            if (accountEntity != null)
            {
                foreach (BalanceDTO balanceDTO in accountEntity.BalancesParsed)
                {
                    var parsedBalance = parsedResponse.Where(b => b.AssetId == balanceDTO.Asset).FirstOrDefault();
                    Assert.NotNull(parsedBalance);
                    Assert.True(balanceDTO.Balance == parsedBalance.Balance);
                }
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetBalancesByAssetId()
        {
            string url = fixture.ApiEndpointNames["Wallets"] + "/balances/" + fixture.TestWalletAssetId;
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
    }
}
