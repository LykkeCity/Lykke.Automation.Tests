using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ApiRestClient;

namespace LykkePay.Resources.PostBack
{
    public class PostBack : RestApi
    {
        public override void SetAllureProperties()
        {
        }

        private string resource = "/getCallBack";
        
        public PostBack() : base("https://lykkepostback.pythonanywhere.com") { }

        public IRestResponse GetCallBackByTransactionID(string tID)
        {
            var request = new RestRequest(resource, Method.GET);
            request.AddQueryParameter("tID", tID);
            var response = client.Execute(request);
            return response;    
        }

        public IRestResponse GetCallBackByOrderID(string order)
        {
            var request = new RestRequest(resource, Method.GET);
            request.AddQueryParameter("orderId", order);
            var response = client.Execute(request);
            return response;
        }
    }
}
