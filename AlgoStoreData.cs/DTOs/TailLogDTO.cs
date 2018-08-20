using Newtonsoft.Json;
using System;

namespace AlgoStoreData.DTOs
{
    public class TailLogDTO
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InstanceId { get; set; }
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }
    }
}
