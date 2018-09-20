using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Catalogs : ApiBase
    {
        public IResponse<List<CountryItem>> GetCatalogsCountries(string token)
        {
            return Request.Get("/Catalogs/countries").WithBearerToken(token).Build().Execute<List<CountryItem>>();
        }
    }
}
