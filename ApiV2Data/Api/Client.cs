using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Client : ApiBase
    {
        public IResponse<AuthResponseModel> PostClientAuth(AuthRequestModel model)
        {
            return Request.Post("/client/auth").AddJsonBody(model).Build().Execute<AuthResponseModel>();
        }

        public IResponse<AccountsRegistrationResponseModel> PostClientRegister(AccountRegistrationModel model)
        {
            return Request.Post("/client/register").AddJsonBody(model).Build().Execute<AccountsRegistrationResponseModel>();
        }

        public IResponse PostClientLogout(string authorization)
        {
            return Request.Post("/client/logout").WithHeaders("Authorization", authorization).Build().Execute();
        }

        public IResponse PatchClientSession(TradingModel model, string authorization)
        {
            return Request.Patch("/client/session").AddJsonBody(model).WithHeaders("Authorization", authorization).Build().Execute();
        }

        public IResponse PostClientSession(TradingModel model, string authorization)
        {
            return Request.Post("/client/session").AddJsonBody(model).WithHeaders("Authorization", authorization).Build().Execute();
        }

        public IResponse<UserInfoResponseModel> GetClientUserInfo(string authorization)
        {
            return Request.Get("/client/userInfo").WithHeaders("Authorization", authorization).Build().Execute<UserInfoResponseModel>();
        }

        public IResponse<FeaturesResponseModel> GetClientFeatures(string authorization)
        {
            return Request.Get("/client/features").WithHeaders("Authorization", authorization).Build().Execute<FeaturesResponseModel>();
        }
    }
}
