using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class AccountExist : ClientAccountBase
    {
        public IResponse<AccountExistResponceModel> GetAccountExist(string email, string partnerId)
        {
            var requset = Request.Get("/api/AccountExist").AddQueryParameter("email", email);
            if (partnerId != null)
                requset.AddQueryParameter("partnerId", partnerId);
            return requset.Build().Execute<AccountExistResponceModel>();
        }
    }
}
