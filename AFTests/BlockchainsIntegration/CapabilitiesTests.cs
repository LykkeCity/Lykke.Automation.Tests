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
                Step("Make GET /cpabilities. Validate response status code is OK", () =>
                {
                    var response = blockchainApi.Capabilities.GetCapabilities();
                    response.Validate.StatusCode(HttpStatusCode.OK);    
                }); 
            }
        }

        public class GetReturnExplorerURL : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetReturnExplorerURLTest()
            {
                var canReturnExplorerUrl = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().CanReturnExplorerUrl;
                if (canReturnExplorerUrl == null || !canReturnExplorerUrl.Value)
                    Assert.Ignore($"Blockchain {BlockChainName} does not support canReturnExplorerUrl");

                string newWallet = "";

                Step("Create new wallet", () => { newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress; });

                Step($"Make GET /addresses/{newWallet}/explorer-url and Validate response contains not empty content", () => 
                {
                    var explorerUrlResponse = blockchainApi.Address.GetAddressExplorerUrl(newWallet);
                    explorerUrlResponse.Validate.StatusCode(HttpStatusCode.OK);

                    Assert.That(explorerUrlResponse.GetResponseObject().Length, Is.GreaterThan(0), "response returned an empty array");
                });               
            }
        }

        public class GetPubicAddressExtension : BlockchainsIntegrationBaseTest
        {
            [Test]
            [Category("BlockchainIntegration")]
            public void GetPubicAddressExtensionTest()
            {
                var canReturnExplorerUrl = blockchainApi.Capabilities.GetCapabilities().GetResponseObject().IsPublicAddressExtensionRequired;
                if (canReturnExplorerUrl == null || !canReturnExplorerUrl.Value)
                    Assert.Ignore($"Blockchain {BlockChainName} does not support public address extension");

                string newWallet = "";

                Step("Create new wallet", () => { newWallet = blockchainSign.PostWallet().GetResponseObject().PublicAddress; });

                Step("Make GET /constants request and validate new wallet address contains AddressExtension Separator", () => 
                {
                    var constantsResponse = blockchainApi.Constants.GetConstants();
                    constantsResponse.Validate.StatusCode(HttpStatusCode.OK);

                    Assert.That(constantsResponse.GetResponseObject().publicAddressExtension.separator, Is.Not.Null.Or.Empty, "Public address separator is null or empty");
                    Assert.That(constantsResponse.GetResponseObject().publicAddressExtension.displayName, Is.Not.Null.Or.Empty, "Display name is null or empty");
                    Assert.That(newWallet, Does.Contain(constantsResponse.GetResponseObject().publicAddressExtension.separator), $"Wallet {newWallet} does not contain separator {constantsResponse.GetResponseObject().publicAddressExtension.separator}");
                });
            }
        }
    }
}
