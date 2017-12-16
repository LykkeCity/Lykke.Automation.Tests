using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class AccountExist : ClientAccountBase
    {
        public IResponse<AccountExistResponceModel> GetAccountExist(string email)
        {
            return Request.Get("/api/AccountExist").AddQueryParameter("email", email).Build()
                .Execute<AccountExistResponceModel>();
        }
    }
}
