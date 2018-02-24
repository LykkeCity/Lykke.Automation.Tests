using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
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
