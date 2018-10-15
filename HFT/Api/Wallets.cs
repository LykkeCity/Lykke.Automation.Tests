namespace HFT.Api
{
    using Lykke.Client.AutorestClient.Models;
    using XUnitTestCommon.RestRequests.Interfaces;

    public class Wallets: ApiBase
    {
        public IResponse<BalanceModel> GetWallets(string apiKey)
        {
            return Request.Get("/Wallets")
                .WithHeaders("api-key", apiKey)
                .Build().Execute<BalanceModel>();
        }
    }
}
