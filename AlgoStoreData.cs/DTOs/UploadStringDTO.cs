using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class UploadStringDTO
    {
        public string Data { get; set; }
    }

    public class PostUploadStringAlgoDTO
    {
        public string Data { get; set; }
        public string AlgoId { get; set; }
    }
}
