using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using TestsCore.RestRequests.Interfaces;
using System.Net;
using Newtonsoft.Json;
using LykkeAutomation.TestsCore;
using System.Linq;
using NUnit.Framework;
using TestsCore.TestsCore;

namespace TestsCore.RestRequests.RestSharpRequest
{
    public class RestSharpRequest : IRequest
    {
        public string BaseUrl { get; }
        public string Resource { get; }
        public Method Method { get; }
        public object JsonBody { get; private set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, object> QueryParams { get; set; }

        private IRestClient client;
        private IRestRequest request;

        public RestSharpRequest(Method method, string baseUrl, string resource)
        {
            Method = method;
            BaseUrl = baseUrl;
            Resource = resource;

            client = new RestClient(BaseUrl);
            request = new RestRequest(Resource, Method);

            if (Environment.OSVersion.ToString().ToLower().Contains("windows"))
                client.Proxy = new WebProxy("127.0.0.1", 8888);

            Headers = new Dictionary<string, string>();
            QueryParams = new Dictionary<string, object>();
        }

        public IResponse Execute()
        {
            var response = client.Execute(request);
            Log(response);
            return new Response() { StatusCode = response.StatusCode, Content = response.Content };
        }

        public IResponse<T> Execute<T>() where T : new()
        {
            var response = client.Execute(request);
            Log(response);
            return new Response<T>() { StatusCode = response.StatusCode, Content = response.Content };
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

        private void Log(IRestResponse response)
        {
            var requestBody = response.Request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);

            string attachName = $"{response.Request.Method} {response.Request.Resource}";
            var attachContext = new StringBuilder();
            attachContext.AppendLine($"Executing {response.Request.Method} {response.ResponseUri}");
            if (requestBody != null)
            {
                attachContext.AppendLine($"Content-Type: {requestBody.ContentType}").AppendLine();
                attachContext.AppendLine(requestBody.Value.ToString());
            }
            attachContext.AppendLine().AppendLine();
            attachContext.AppendLine($"Response: {response.StatusCode}");
            if (response.ErrorMessage != null)
                attachContext.AppendLine(response.ErrorMessage);
            attachContext.AppendLine(response.Content);
            //TestLog.WriteLine(attachContext.ToString());
            Allure2Helper.AttachJson(attachName, attachContext.ToString());
           // AllureReport.GetInstance().AddAttachment(TestContext.CurrentContext.Test.FullName,
           //  Encoding.UTF8.GetBytes(attachContext.ToString()), attachName, "application/json");
        }
    }
}
