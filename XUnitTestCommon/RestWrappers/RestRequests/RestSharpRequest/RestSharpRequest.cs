using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using XUnitTestCommon.RestRequests.Interfaces;
using System.Net;
using Newtonsoft.Json;
using LykkeAutomation.TestsCore;
using System.Linq;
using NUnit.Framework;
using XUnitTestCommon.TestsCore;
using XUnitTestCommon.Reports;
using System.Net.Sockets;

namespace XUnitTestCommon.RestRequests.RestSharpRequest
{
    public class RestSharpRequest : IRequest
    {
        public string BaseUrl { get; }
        public string Resource { get; }
        public Method Method { get; }
        public object JsonBody { get; private set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, object> QueryParams { get; set; }
        public IList<Parameter> Cookies { get; set; }

        private IRestClient client;
        private IRestRequest request;

        public RestSharpRequest(Method method, string baseUrl, string resource, bool autoredirect = true)
        {
            Method = method;
            BaseUrl = baseUrl;
            Resource = resource;

            client = new RestClient(BaseUrl);
            client.FollowRedirects = autoredirect;
            request = new RestRequest(Resource, Method);

            //set proxy for local debug
            try
            {
                new TcpClient().Connect("127.0.0.1", 8888);
                client.Proxy = new WebProxy("127.0.0.1", 8888);
            }
            catch (Exception)
            {
                //do nothing
            }

            Headers = new Dictionary<string, string>();
            QueryParams = new Dictionary<string, object>();
        }

        //public RestSharpRequest(Method method, string baseUrl, string resource)
        //{
        //    Method = method;
        //    BaseUrl = baseUrl;
        //    Resource = resource;

        //    client = new RestClient(BaseUrl);
        //    request = new RestRequest(Resource, Method);

        //    //set proxy for local debug
        //    try
        //    {
        //        new TcpClient().Connect("127.0.0.1", 8888);
        //        client.Proxy = new WebProxy("127.0.0.1", 8888);
        //    }
        //    catch (Exception)
        //    {
        //        //do nothing
        //    }
            
        //    Headers = new Dictionary<string, string>();
        //    QueryParams = new Dictionary<string, object>();
        //}

        public IResponse Execute()
        {
            var response = client.Execute(request);
            Log(response);
            return new Response()
            { StatusCode = response.StatusCode, Content = response.Content, Headers = response.Headers, ResponseURI = response.ResponseUri, Cookies = response.Headers.ToList().FindAll(h => h.Name == "Set-Cookie") };
        }

        public IResponse<T> Execute<T>()
        {
            var response = client.Execute(request);
            Log(response);
            return new Response<T>()
            { StatusCode = response.StatusCode, Content = response.Content, Headers = response.Headers, ResponseURI = response.ResponseUri, Cookies = response.Headers.ToList().FindAll(h => h.Name == "Set-Cookie") };
        }

        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
            request.AddHeader(name, value);
        }

        public void AddQueryParameter(string name, object value)
        {
            QueryParams.Add(name, value);
            request.AddParameter(name, value, ParameterType.QueryString);
        }

        public void AddTextBody(string text)
        {
            request.AddParameter("text/xml", text, ParameterType.RequestBody);
        }

        public void AddObject(object body)
        {
            request.AddObject(body);
        }

        public void AddJsonBody(object json)
        {
            JsonBody = json;

            if (JsonBody != null)
            {
                var settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
                string jsonStr = JsonConvert.SerializeObject(JsonBody, settings);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json", jsonStr, "application/json", ParameterType.RequestBody);
            }
        }

        public void AddJsonBody(string json)
        {
            JsonBody = json;

            if (JsonBody != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json", json, "application/json", ParameterType.RequestBody);
            }
        }

        private void Log(IRestResponse response)
        {
            var requestBody = response.Request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);

            var requestHeaders = response.Request.Parameters.FindAll(p => p.Type == ParameterType.HttpHeader);
            string attachName = $"{DateTime.UtcNow.ToString("s")}(UTC) {response.Request.Method} {response.Request.Resource}";
            var attachContext = new StringBuilder();
            attachContext.AppendLine($"Executing {response.Request.Method} {response.ResponseUri}");
            if (requestBody != null)
            {
                attachContext.AppendLine($"Content-Type: {requestBody.ContentType}").AppendLine();
                attachContext.AppendLine(requestBody.Value.ToString());
            }
            requestHeaders.ForEach(h => attachContext.AppendLine(h.Name + ": " + h.Value));

            attachContext.AppendLine().AppendLine();
            attachContext.AppendLine($"Response: {response.StatusCode}");
            if (response.ErrorMessage != null)
                attachContext.AppendLine(response.ErrorMessage);
            attachContext.AppendLine(response.Content);
            Allure2Helper.AttachJson(attachName, attachContext.ToString());
        }

        private void LogToStep(IRestResponse response)
        {
            var requestBody = response.Request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);

            var requestHeaders = response.Request.Parameters.FindAll(p => p.Type == ParameterType.HttpHeader);
            string attachName = $"{DateTime.UtcNow.ToString("s")}(UTC) {response.Request.Method} {response.Request.Resource}";
            var attachContext = new StringBuilder();
            attachContext.AppendLine($"Executing {response.Request.Method} {response.ResponseUri}");
            if (requestBody != null)
            {
                attachContext.AppendLine($"Content-Type: {requestBody.ContentType}").AppendLine();
                attachContext.AppendLine(requestBody.Value.ToString());
            }
            requestHeaders.ForEach(h => attachContext.AppendLine(h.Name + ": " + h.Value));

            attachContext.AppendLine().AppendLine();
            attachContext.AppendLine($"Response: {response.StatusCode}");
            if (response.ErrorMessage != null)
                attachContext.AppendLine(response.ErrorMessage);
            attachContext.AppendLine(response.Content);
            Allure2Helper.AttachJson(attachName, attachContext.ToString());
        }
    }
}
