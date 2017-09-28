using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.DTOs
{
    public class ResultDTO
    {
        public string KycStatus { get; set; }
        public bool PinIsEntered { get; set; }
        public string Token { get; set; }
        public string NotificationsId { get; set; }
        public PersonalDataDTO PersonalData { get; set; }
        public bool CanCashInViaBankCard { get; set; }
        public bool SwiftDepositEnabled { get; set; }
        public bool IsUserFromUSA { get; set; }

    }
}
