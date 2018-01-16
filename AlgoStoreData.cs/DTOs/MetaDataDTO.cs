using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    public class MetaDataDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MetaDataResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
    }

    public class MetaDataEditDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
    }

}
