using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;
using LykkeAutomationPrivate.Models.ClientAccount.Models;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class ClientAccount : ClientAccountBase
    {
        public IResponse<ErrorResponse> DeleteClientAccount(string id)
        {
            return Request.Delete($"/api/ClientAccount/{id}").Build().Execute<ErrorResponse>();
        }

        public IResponse<ErrorResponse> PutClientAccountEmail(string id, string email)
        {
            return Request.Put($"/api/ClientAccount/{id}/email/{email}").Build().Execute<ErrorResponse>();
        }

        public IResponse<bool> GetClientAccountTrusted(string id)
        {
            return Request.Get($"/api/ClientAccount/{id}/trusted").Build().Execute<bool>();
        }

        public IResponse<Int32> GetUsersCountByPartnerId(string partnerId)
        {
            return Request.Get("/api/ClientAccount/getUsersCountByPartnerId")
                .AddQueryParameter("partnerId", partnerId).Build().Execute<Int32>();
        }
    }
}
