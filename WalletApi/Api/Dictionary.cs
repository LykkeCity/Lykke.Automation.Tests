using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class Dictionary : ApiBase
    {

        public IResponse<ResponseModelIKeyValue2> GetDictionary()
        {
            return Request.Get("/Dictionary").Build().Execute<ResponseModelIKeyValue2>();
        }

        public IResponse<ResponseModelIKeyValue> GetDictionaryKey(string key)
        {
            return Request.Get($"/Dictionary/{key}").Build().Execute<ResponseModelIKeyValue>();
        }
    }
}
