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
    public class IronCladApi
    {
        private string _guid;

        protected string URL() => $"https://ironclad-decorator-test.lykkex.net/connect/authorize?client_id=bdba77e6-d9ed-44df-9a36-c2ecf07d97b6&scope=profile%20email%20address&response_type=token&redirect_uri=https%3A%2F%2Fwebwallet-test.lykkex.net%2Fauth&nonce={TestData.GenerateString(20)}&state={TestData.GenerateString(20)}";

        protected string URL(string _once, string _state) => $"https://ironclad-decorator-test.lykkex.net/connect/authorize?client_id=bdba77e6-d9ed-44df-9a36-c2ecf07d97b6&scope=profile%20email%20address&response_type=token&redirect_uri=https%3A%2F%2Fwebwallet-test.lykkex.net%2Fauth&nonce={_once}&state={_state}";

        protected string _once = TestData.GenerateString(20);
        protected string _state = TestData.GenerateString(20);// "FlAZblzwVGcFwigsX4UI";

        public IRequestBuilder Request => Requests.For(URL());

        public IronCladApi() { _guid = Guid.NewGuid().ToString(); }

        public IResponse GetIroncladAuthorize(string _once, string _state)
        {
            return Requests.For(URL(_once, _state)).Get("", false).Build().Execute();
        }
    }
}
