using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class AccountExist : ApiBase
    {
        private string resource = "/AccountExist";

        public IResponse<ResponseModelAccountExistResultModel> GetAccountExistResponse(string email)
        {
            return Request.Get(resource).AddQueryParameter("email", email).Build().Execute<ResponseModelAccountExistResultModel>();
        }
    }
}
