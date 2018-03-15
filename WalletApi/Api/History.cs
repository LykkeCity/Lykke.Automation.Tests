using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class History : ApiBase
    {
        /// <summary>
        /// -
        /// </summary>
        /// <param name="assetId">Empty string for all history</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public IResponse<ResponseModelIEnumerableHistoryRecordModel> GetByAssetId(string assetId, string token) =>
            Request.Get("/History").AddQueryParameter(nameof(assetId), assetId).WithBearerToken(token)
                .Build().Execute<ResponseModelIEnumerableHistoryRecordModel>();

        //TODO: Add other
    }
}
