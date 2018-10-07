using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ApiV2Data.Models
{
    public class TradingSessionConfirmModel
    {
        [JsonProperty(PropertyName = "Confirmation")]
        public string Confirmation { get; set; }

        [JsonProperty(PropertyName = "SessionId")]
        public string SessionId { get; set; }

        public TradingSessionConfirmModel(string confirmation, string sessionId)
        {
            Confirmation = confirmation;
            SessionId = sessionId;
        }

        public TradingSessionConfirmModel()
        {
        }
    }
}
