using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models.ResponseModels
{
    public class PostConvertTransferResponseModel
    {
        public TransferStatus transferStatus { get; set; }
        public TransferResponse transferResponse { get; set; }
    }

    public enum TransferStatus
    {
        TRANSFER_INPROGRESS,
        TRANSFER_CONFIRMED,
        TRANSFER_ERROR
    }
}
