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

        public JObject JObject => JObject.Parse(Content);

        public IValidate Validate => new Validate(this);
    }

    public class Response<T> : Response, IResponse<T>
    {
        private T responseObject;

        IValidate<T> IResponse<T>.Validate => new Validate<T>(this);

        public T ResponseObject
        {
            get
            {
                if(Equals(responseObject, default(T)))
                    responseObject = JsonConvert.DeserializeObject<T>(Content);

                return responseObject;
            }
        }

        public T GetResponseObject() => ResponseObject;
    }
}
