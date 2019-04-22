using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;
using XUnitTestCommon.TestsData;

namespace ApiV2Data.Api
{
    public class LykkeTechAPI
    {
        private string _guid;

        protected string URL(string _once, string _state) => $"https://lykke.tech/signin?returnurl=%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3D{_guid}%26scope%3Demail%2520openid%26response_type%3Dcode%26redirect_uri%3Dhttps%253A%252F%252Fironclad-decorator-test.lykkex.net%252Fcallback%252Fsignin-oidc%26nonce%3D{_once}%26state%3D{_state}%26client_secret%3Dsecret%26x-client-SKU%3DID_NETSTANDARD1_4%26x-client-ver%3D5.2.0.0";


        protected string _once = TestData.GenerateString(20);
        protected string _state = TestData.GenerateString(20);// "FlAZblzwVGcFwigsX4UI";

        public LykkeTechAPI() { _guid = Guid.NewGuid().ToString(); }


        public IResponse PostSignIn(string URL, string cookie, string RequestVerificationToken, string userName, string password)
        {
            string o = $"Email={userName}&Password={password}&__RequestVerificationToken={RequestVerificationToken}&RememberMe=false";

            return Requests.For(URL.Replace("return_url", "returnurl")).Post("", false).AddTextBody(o).WithHeaders("Cookie", cookie).WithHeaders("Content-Type", "application/x-www-form-urlencoded").Build().Execute();

        }

        public IResponse GetConnect(string URL, string cookie)
        {
            return Requests.For(URL).Get("", false).WithHeaders("Cookie", cookie).WithHeaders("Content-Type", "application/x-www-form-urlencoded").Build().Execute();

        }

        public IResponse GetConnect(string URL, string cookie, string bearerAuthToken)
        {
            return Requests.For(URL).Get("", false).WithHeaders("Cookie", cookie).WithHeaders("Content-Type", "application/x-www-form-urlencoded").WithBearerToken(bearerAuthToken).Build().Execute();

        }
    }
}
