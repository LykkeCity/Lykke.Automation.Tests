using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class VerifiedEmails : ClientAccountBase
    {
        public IResponse PostVerifiedEmails(VerifiedEmailModel verifiedEmail) =>
            Request.Post("/api/VerifiedEmails").AddJsonBody(verifiedEmail)
            .Build().Execute();

        public IResponse PutVerifiedEmails(string email, VerifiedEmailModel verifiedEmail) =>
            Request.Put($"/api/VerifiedEmails/{email}").AddJsonBody(verifiedEmail)
            .Build().Execute();

        public IResponse DeleteVerifiedEmails(VerifiedEmailModel verifiedEmail) =>
            Request.Delete("/api/VerifiedEmails").AddJsonBody(verifiedEmail)
            .Build().Execute();
    }
}
