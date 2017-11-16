using System;
using System.Collections.Generic;
using System.Text;

namespace BlueApiData.DTOs
{
    public class TweetDTO
    {
        public string TweetId { get; set; }
        public string AccountId { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string UserImage { get; set; }
        public string TweetImage { get; set; }
    }
}
