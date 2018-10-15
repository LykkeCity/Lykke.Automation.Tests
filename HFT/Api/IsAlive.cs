namespace HFT.Api
{
    using Lykke.Client.AutorestClient.Models;
    using XUnitTestCommon.RestRequests.Interfaces;

    public class IsAlive : ApiBase
    {
        public IResponse<IsAliveResponse> GetIsAlive()
        {
            return Request.Get("/IsAlive").Build().Execute<IsAliveResponse>();
        }
    }
}
