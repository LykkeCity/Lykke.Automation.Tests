using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.RestRequests.RestSharpRequest;

namespace XUnitTestCommon.RestRequests
{
    public static class Requests
    {
        public static IRequestBuilder For(string baseUrl)
        {
            return new RestSharpRequestBuilder(baseUrl);
        }
    }
}
