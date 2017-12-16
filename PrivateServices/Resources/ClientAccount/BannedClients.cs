using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests.Interfaces;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using System.Linq;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class BannedClients : ClientAccountBase
    {
        public IResponse PutBannedClients(string clientId)
        {
            return Request.Put($"/api/BannedClients/{clientId}").Build().Execute();
        }

        public IResponse DeleteBannedClients(string clientId)
        {
            return Request.Delete($"/api/BannedClients/{clientId}").Build().Execute();
        }

        public IResponse<List<BannedClient>> GetBannedClients()
        {
            return Request.Get("/api/BannedClients").Build().Execute<List<BannedClient>>();
        }

        public IResponse<bool> GetBannedClientsIsBanned(string clientId)
        {
            return Request.Get($"/api/BannedClients/{clientId}/isBanned").Build().Execute<bool>();
        }

        public IResponse<List<BannedClient>> PostBannedCleintsFilterByIds(List<string> clientsIds)
        {
            var request = Request.Post("/api/BannedClients/filterByIds");
            if (clientsIds != null && clientsIds.Any())
                request.AddJsonBody(clientsIds);
            else
                request.AddJsonBody(new object());

            return request.Build().Execute<List<BannedClient>>();
        }
    }
}
