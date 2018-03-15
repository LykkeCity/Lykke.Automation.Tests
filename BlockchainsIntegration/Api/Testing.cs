using BlockchainsIntegration.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.Api
{
    public class Testing : ApiBase
    {
        public Testing(string URL) : base(URL)
        {
        }

        public IResponse PostTestingTransfer(TestingTransferRequest request)
        {
            return Request.Post("/testing/transfers").AddJsonBody(request).Build().Execute();
        }
    }
}
