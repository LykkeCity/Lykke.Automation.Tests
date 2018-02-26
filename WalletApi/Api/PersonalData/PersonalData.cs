using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.TestsCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;

namespace WalletApi.Api.PersonalDataResource
{
   public class PersonalData : ApiBase
    {
        private const string resource = "/PersonalData";

        public IResponse<ResponseModelApiPersonalDataModel> GetPersonalDataResponse(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelApiPersonalDataModel>();
        }
    }
}
