using LykkeAutomation.Api.ApiModels.ErrorModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LykkeAutomation.Api.ApiModels.AccountExistModels
{

    public class AccountExistModel
    {
        public Result Result { get; set; }
        public Error Error { get; set; }

        //think about generic
        public static AccountExistModel ConvertToAccountExistModel(string json) => JsonConvert.DeserializeObject<AccountExistModel>(json);
    }

    public class Result
    {
        public bool IsEmailRegistered { get; set; }
    }


    
    
}