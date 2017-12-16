using TestsCore.RestRequests.Interfaces;
using TestsCore.RestRequests.RestSharpRequest;

namespace TestsCore.RestRequests
{
    public static class Requests
    {
        public static IRequestBuilder For(string baseUrl)
        {
            return new RestSharpRequestBuilder(baseUrl);
        }
    }
}
