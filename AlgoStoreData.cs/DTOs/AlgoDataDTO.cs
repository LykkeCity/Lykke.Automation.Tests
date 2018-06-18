using System;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs
{
    public class AlgoDataDTO
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public AlgoVisibility AlgoVisibility { get; set; }
    }
}
