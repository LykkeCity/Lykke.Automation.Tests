using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class CountryPhoneCodes : ApiBase
    {
        string resource = "/CountryPhoneCodes";

        public IResponse<ResponseModelCountriesResponseModel> GetCountryPhoneCodes(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelCountriesResponseModel>();
        }
    }
}
