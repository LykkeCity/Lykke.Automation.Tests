using XUnitTestCommon.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace XUnitTestCommon
{
    public class Response
    {
        public HttpStatusCode Status { get; set; }
        public String ResponseJson { get; set; }

        public Response() { }

        public Response(HttpStatusCode statusCode, string responseJson)
        {
            this.Status = statusCode;
            this.ResponseJson = responseJson;
        }
    }
}
