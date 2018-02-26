using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class GetAlgoMetaDataDTO
    {
        public string AlgoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
        public double Rating { get; set; }
        public int UsersCount { get; set; }
        public Hashtable AlgoMetaDataInformation { get; set; }
    }
}
