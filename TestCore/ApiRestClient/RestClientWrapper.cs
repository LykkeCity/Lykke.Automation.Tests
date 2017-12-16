using LykkeAutomation.TestsCore;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LykkeAutomation.TestsCore
{
    public class RestClientWrapper : RestClient
    {

        public IRestResponse Execute(IRestRequest request, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            TestLog.WriteStep($"Execute request name: {memberName}");
            var response = base.Execute(request);

            AddToLog(response);
            return response;
        }

        private void AddToLog(IRestResponse response)
        {
            TestLog.WriteLine(ResponseInfo(response.ResponseUri.AbsoluteUri, response.Request));
            TestLog.WriteLine(ResponseInfo(response));
        }

        private string ResponseInfo(string URL, IRestRequest request)
        {
            string result = "";
            result += request.Method + "\r\n";
            result += URL + "\r\n";
            request.Parameters.FindAll(p => p.Type != ParameterType.RequestBody).ForEach(p => result += p.Name + ": " + p.Value + "\r\n");
            result += request.Parameters?.Find(p => p.Type == ParameterType.RequestBody)?.Value + "\r\n";
            return result;
        }

        private string ResponseInfo(IRestResponse response)
        {
            string result = "";
            result += response.ResponseUri + "\r\n";
            result += response.StatusCode + "\r\n";
            response.Headers.ToList().ForEach(p => result += p.Name + ": " + p.Value + "\r\n");
            result += response.Content + "\r\n";
            return result;
        }

        public RestClientWrapper(string URL) : base(URL)
        { }
    }
}
