using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.RestRequests;
using TestsCore.RestRequests.Interfaces;

namespace LykkePay.Resources.PostBack
{
    public class PostBack
    {
        private string resource = "/getCallBack";

        string URL = "https://lykkepostback.pythonanywhere.com";

        public IResponse GetCallBackByTransactionID(string tID)
        {
            return Requests.For(URL).Get(resource).AddQueryParameter("tID", tID).Build().Execute();
        }

        public IResponse GetCallBackByOrderID(string order)
        {
            return Requests.For(URL).Get(resource).AddQueryParameter("orderId", order).Build().Execute();
        }
    }
}
