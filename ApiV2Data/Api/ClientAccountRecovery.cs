using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class ClientAccountRecovery : ApiBase
    {
        public IResponse<RecoveryStartResponseModel> PostAccountRecoveryStart(RecoveryStartRequestModel  model)
        {
            return Request.Post("/account/recovery/start").AddJsonBody(model).Build().Execute<RecoveryStartResponseModel>();
        }

        public IResponse<RecoveryStatusResponseModel> GetAccountRecoveryStatus(string accountRecoveryToken)
        {
            return Request.Get("/account/recovery/status").AddQueryParameter("StateToken", accountRecoveryToken).Build().Execute<RecoveryStatusResponseModel>();
        }

        public IResponse<RecoverySubmitChallengeResponseModel> PostAccountRecoveryChallenge(RecoverySubmitChallengeRequestModel model)
        {
            return Request.Post("/account/recovery/challenge").AddJsonBody(model).Build().Execute<RecoverySubmitChallengeResponseModel>();
        }

        public IResponse<RecoveryUploadFileResponseModel> PostRecoveryFile(string file, string StateToken)
        {
            return Request.Post("/account/recovery/file").AddQueryParameter("File", file).AddQueryParameter("StateToken", StateToken).Build().Execute<RecoveryUploadFileResponseModel>();
        }

        public IResponse PostAccountRecoveryPassword(RecoveryCompleteRequestModel model)
        {
            return Request.Post("/account/recovery/password").AddJsonBody(model).Build().Execute();
        }
    }
}
