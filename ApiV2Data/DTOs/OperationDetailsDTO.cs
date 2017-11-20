using System;
using System.Collections.Generic;
using System.Text;

namespace ApiV2Data.DTOs
{
    public class OperationDetailsDTO
    {
        public string TransactionId { get; set; }
        public string Comment { get; set; }
    }

    public class OperationDetailsReturnDTO
    {
        public string Id { get; set; }
    }
}
