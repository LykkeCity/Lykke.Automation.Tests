using System;
using System.Collections.Generic;
using System.Text;
using ApiV2Data.Models;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class SecondFactorAuth : ApiBase
    {
        public IResponse Post2FAOperation(OperationConfirmationModel model, string token)
        {
            return Request.Post("/2fa/operation").WithBearerToken(token).Build().Execute();
        }

        public IResponse<List<string>> Get2FA(string token)
        {
            return Request.Get("/2fa").WithBearerToken(token).Build().Execute<List<string>>();
        }

        public IResponse<GoogleSetupRequestResponse> Get2FASetUpGoogle()
        {
            return Request.Get("/2fa/setup/google").Build().Execute<GoogleSetupRequestResponse>();
        }

        public IResponse<GoogleSetupVerifyResponse> Post2FASetUpGoogle(GoogleSetupVerifyRequest model, string token)
        {
            return Request.Post("/2fa/setup/google").AddJsonBody(model).WithBearerToken(token).Build().Execute<GoogleSetupVerifyResponse>();
        }
    }
}
