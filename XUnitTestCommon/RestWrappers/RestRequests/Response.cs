using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using XUnitTestCommon.RestRequests.Interfaces;

namespace XUnitTestCommon.RestRequests
{
    public class Response : IResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Content { get; set; }

        public JObject JObject
        {
            get
            {
                return JObject.Parse(Content);
            }
        }
    }


    public class Response<T> : Response, IResponse<T>
    {
        public T GetResponseObject()
        {
            return JsonConvert.DeserializeObject<T>(Content);
        }
    }
}
