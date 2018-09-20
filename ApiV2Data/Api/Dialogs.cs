using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Dialogs : ApiBase
    {
        public IResponse<ClientDialogResponseModel> GetDialogs(string token)
        {
            return Request.Get("/dialogs").WithBearerToken(token).Build().Execute<ClientDialogResponseModel>();
        }

        public IResponse PostDialogIdActionsActionId(string dialogId, string actionId, string token)
        {
            return Request.Post($"/dialogs/{dialogId}/actions/{actionId}").WithBearerToken(token).Build().Execute();
        }
    }
}
