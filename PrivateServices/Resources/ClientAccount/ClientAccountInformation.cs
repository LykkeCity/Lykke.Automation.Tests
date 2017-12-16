using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class ClientAccountInformationResource : ClientAccountBase
    {
        public IResponse<ClientAccountInformation> GetClientAccountInformation(string id)
        {
            return Request.Get("/api/ClientAccountInformation").AddQueryParameter("id", id)
                .Build().Execute<ClientAccountInformation>();
        }

        public IResponse<List<ClientAccountInformation>> GetClientsByIds(ClientAccountIdsModel ids)
        {
            //TODO: Body in GET does not alowed https://msdn.microsoft.com/en-us/library/d4cek6cc%28v=vs.110%29.aspx
            throw new NotSupportedException("Body in GET does not alowed");
        }

        public IResponse<List<string>> GetClientsByPhone(string phoneNumber)
        {
            return Request.Get($"/api/ClientAccountInformation/getClientsByPhone/{phoneNumber}")
                .Build().Execute<List<string>>();
        }

        public IResponse<bool> GetIsPasswordCorrect(string clientId, string password)
        {
            return Request.Get($"/api/ClientAccountInformation/isPasswordCorrect/{clientId}/{password}")
                .Build().Execute<bool>();
        }

        public IResponse<ClientResponseModel> GetClientById(string clientId)
        {
            return Request.Get("/api/ClientAccountInformation/getClientById")
                .AddQueryParameter("id", clientId)
                .Build().Execute<ClientResponseModel>();
        }

        public IResponse<List<ClientAccountInformation>> GetClientsByEmail(string email)
        {
            return Request.Get($"/api/ClientAccountInformation/getClientsByEmail/{email}")
                .Build().Execute<List<ClientAccountInformation>>();
        }

        public IResponse<ClientAccountInformation> GetClientByEmailAndPartnerId(string email, string partnerId)
        {
            return Request.Get($"/api/ClientAccountInformation/getClientByEmailandPartnerId/{email}")
                .AddQueryParameter("partnerId", partnerId).Build().Execute<ClientAccountInformation>();
        }

        public IResponse<ClientAccountInformation> PostAuthenticate(ClientAuthenticationModel auth)
        {
            return Request.Post("/api/ClientAccountInformation/authenticate").AddJsonBody(auth)
                .Build().Execute<ClientAccountInformation>();
        }

        public IResponse PostSetPIN(string clientId, string pin)
        {
            return Request.Post($"/api/ClientAccountInformation/setPIN/{clientId}/{pin}").Build().Execute();
        }

        public IResponse PostChangeClientPassword(PasswordHashModel passwordHashModel)
        {
            return Request.Post("/api/ClientAccountInformation/changeClientPassword")
                .AddJsonBody(passwordHashModel).Build().Execute();
        }
    }
}
