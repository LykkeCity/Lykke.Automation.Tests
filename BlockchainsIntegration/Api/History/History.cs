using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace BlockchainsIntegration.Api
{
    public class History : ApiBase
    {
        public History() : base() { }
        public History(string BaseUrl) : base(BaseUrl) { }

        public IResponse<IList<HistoricalTransactionContract>> GetHistoryFromToAddress(string fromTo, string address, string afterHash = null, string take = null)
        {
            return Request.Get($"/history/{fromTo}/{address}").
                AddQueryParameterIfNotNull("afterHash", afterHash).AddQueryParameterIfNotNull("take", take).Build().Execute<IList<HistoricalTransactionContract>>();
        }

        public IResponse PostHistoryFromToAddress(string fromTo, string address)
        {
            return Request.Post($"/history/{fromTo}/{address}/observation").Build().Execute();
        }   
    }
}
