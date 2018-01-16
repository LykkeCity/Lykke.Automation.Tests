using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.LiteCoin.Api
{
    public class Address : ApiBase
    {
        public IResponse<AddressValidationResponse> GetAddress(string address)
        {
            return Request.Get($"/addresses/{address}/address-validity").Build().Execute<AddressValidationResponse>();
        }
    }
}
