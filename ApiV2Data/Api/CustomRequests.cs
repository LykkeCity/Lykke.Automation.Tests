using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class CustomRequests : ApiBase
    {
        public IResponse GetResponse(string GETUrl) => Requests.For(GETUrl).Get("").Build().Execute();
        public IResponse PostResponse(string PostUrl) => Requests.For(PostUrl).Post("").Build().Execute();
        public IResponse PostResponse(string PostUrl, string Json) => Requests.For(PostUrl).Post("").AddJsonBody(Json).Build().Execute();
    }
}
