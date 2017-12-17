using LykkeAutomationPrivate.Models.ClientAccount.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class Clients : ClientAccountBase
    {
        public IResponse<ClientAccountInformation> PostRegister(ClientRegistrationModel clientRegistrationModel)
        {
            return Request.Post("/api/Clients/register")
                .AddJsonBody(clientRegistrationModel).Build().Execute<ClientAccountInformation>();
        }

        public IResponse<string> GenerateNotificationsId(string clientId)
        {
            return Request.Post($"/api/Clients/{clientId}/generateNotificationsId").Build()
                .Execute<string>();
        }
    }
}
