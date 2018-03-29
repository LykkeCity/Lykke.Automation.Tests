using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LiteCoin.Api
{
    public class Address : ApiBase
    {
        public Address(string url) : base(url) { }
        public Address() : base() { }

        public IResponse<AddressValidationResponse> GetAddress(string address)
        {
            return Request.Get($"/addresses/{address}/validity").Build().Execute<AddressValidationResponse>();
        }

        public IResponse<string[]> GetAddressExplorerUrl(string address)
        {
            return Request.Get($"addresses/{address}/explorer-url").Build().Execute<string[]>();
        }
    }
}
