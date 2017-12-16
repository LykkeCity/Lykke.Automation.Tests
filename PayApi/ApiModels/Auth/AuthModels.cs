using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LykkeAutomation.ApiModels.PersonalDataModels;
using LykkeAutomation.Api.ApiModels.ErrorModel;
using LykkeAutomation.ApiModels.RegistrationModels;
using Newtonsoft.Json;

namespace LykkeAutomation.Api.ApiModels.AuthModels
{
    public class AuthModels
    {

        public class AuthenticateModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ClientInfo { get; set; }
            public string PartnerId { get; set; }

            public AuthenticateModel(AccountRegistrationModel arm)
            {
                this.Email = arm.Email;
                this.Password = arm.Password;
                this.ClientInfo = arm.ClientInfo;
                this.PartnerId = arm.PartnerId;
            }

            public AuthenticateModel(string Email, string Password, string ClientInfo, string PartnerId = "")
            {
                this.Email = Email;
                this.Password = Password;
                this.ClientInfo = ClientInfo;
                this.PartnerId = PartnerId;
            }

        }

        public class AuthModelResponse
        {
            public Result Result { get; set; }
            public Error Error { get; set; }

        }

        public class Result
        {
            public string KycStatus { get; set; }
            public bool PinIsEntered { get; set; }
            public string Token { get; set; }
            public string NotificationsId { get; set; }
            public PersonalData PersonalData { get; set; }
            public bool CanCashInViaBankCard { get; set; }
            public bool SwiftDepositEnabled { get; set; }
            public bool IsUserFromUSA { get; set; }
        }

        public static AuthModelResponse ConvertToAuthModelResponse(string json) => JsonConvert.DeserializeObject<AuthModelResponse>(json);        
    }
}
