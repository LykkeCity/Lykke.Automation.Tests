using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class IsAlive : ClientAccountBase
    {
        public IResponse GetIsAlive()
        {
            return Request.Get("/api/IsAlive").Build().Execute();
        }
    }
}
