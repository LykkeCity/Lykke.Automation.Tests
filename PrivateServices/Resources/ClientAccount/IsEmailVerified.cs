using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class IsEmailVerified : ClientAccountBase
    {
        public IResponse<bool> PostIsEmailVerified(VerifiedEmailModel verifiedEmail)
        {
            return Request.Post("/api/IsEmailVerified").AddJsonBody(verifiedEmail)
                .Build().Execute<bool>();
        }
    }
}
