using AFTests.BlockchainsIntegrationTests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.BlockchainsIntegrationTests
{
    class CapabilitiesTests 
    {
        public class GetCapabilities : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetCapabilitiesTest()
            {
                var response = blockchainApi.Capabilities.GetCapabilities();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().IsTransactionsRebuildingSupported, Is.False.Or.False);
            }
        }

        public class GetReturnExplorerURL : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetReturnExplorerURLTest()
            {
                var canReturnExplorerUrl = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().CanReturnExplorerUrl;
                if (canReturnExplorerUrl == null || !canReturnExplorerUrl.HasValue)
                    Assert.Ignore($"Blockchain {BlockChainName} does not support canReturnExplorerUrl");

                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                var explorerUrlResponse = blockchainApi.Address.GetAddressExplorerUrl(newWallet);
                explorerUrlResponse.Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(explorerUrlResponse.GetResponseObject().Length, Is.GreaterThan(0), "response returned an empty array");
            }
        }

        public class GetPubicAddressExtension : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetPubicAddressExtensionTest()
            {
                var canReturnExplorerUrl = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired;
                if (canReturnExplorerUrl == null || !canReturnExplorerUrl.HasValue)
                    Assert.Ignore($"Blockchain {BlockChainName} does not support public address extension");

                var newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress;

                var constantsResponse = blockchainApi.Constants.GetConstants();
                constantsResponse.Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(constantsResponse.GetResponseObject().publicAddressExtension.separator, Is.Not.Null.Or.Empty, "Public address separator is null or empty");
                Assert.That(constantsResponse.GetResponseObject().publicAddressExtension.displayName, Is.Not.Null.Or.Empty, "Display name is null or empty");
                Assert.That(newWallet, Does.Contain(constantsResponse.GetResponseObject().publicAddressExtension.separator), $"Wallet {newWallet} does not contain separator {constantsResponse.GetResponseObject().publicAddressExtension.separator}");
            }
        }
    }
}
