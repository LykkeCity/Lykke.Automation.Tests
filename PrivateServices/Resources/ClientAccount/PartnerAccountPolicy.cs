using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class PartnerAccountPolicy : ClientAccountBase
    {
        public IResponse<IPartnerAccountPolicy> GetPartnerAccountPolicy(string partnerPublicId)
        {
            return Request.Get("/api/PartnerAccountPolicy").AddQueryParameter("partnerPublicId", partnerPublicId)
                .Build().Execute<IPartnerAccountPolicy>();
        }
    }
}
