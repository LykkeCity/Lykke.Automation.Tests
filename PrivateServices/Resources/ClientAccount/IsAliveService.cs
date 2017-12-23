using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class IsAliveService : ClientAccountBase
    {
        public IResponse<IsAliveResponse> GetIsAliveService()
        {
            return Request.Get("/api/IsAliveService").Build().Execute<IsAliveResponse>();
        }
    }
}
