using System;
using System.Collections.Generic;
using System.Text;

namespace BlueApiData.DTOs
{
    public class TweetDTO
    {
        public string full_text { get; set; }
        public DateTime created_at { get; set; }
    }
}
