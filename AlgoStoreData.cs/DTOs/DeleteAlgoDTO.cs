using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class DeleteAlgoDTO
    {
        public string AlgoId { get; set; }
        public string AlgoClientId { get; set; }
        public bool ForceDelete { get; set; }
    }
}
