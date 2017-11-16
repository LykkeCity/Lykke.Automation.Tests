using System;
using System.Collections.Generic;
using System.Text;

namespace BlueApiData.DTOs
{
    public class TwitterSearchDTO
    {
        public string Id { get; set; }
        public bool IsExtendedSearch { get; set; }
        public string AccountEmail { get; set; }
        public string SearchQuery { get; set; }
        public int MaxResult { get; set; }
        public DateTime UntilDate { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
