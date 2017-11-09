using System;
using System.Collections.Generic;
using System.Text;

namespace ApiV2Data.DTOs
{
    public class OperationDTO
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }
        public string ClientId { get; set; }
        public OperationContext Context { get; set; }
        public string Type { get; set; }
    }

    public class OperationContext
    {
        public string AssetId { get; set; }
        public double Amount { get; set; }
        public string SourceWalletId { get; set; }
        public string WalletId { get; set; }
        public string TransferType { get; set; }
    }

    public class OperationCreateDTO
    {
        public string AssetId { get; set; }
        public double Amount { get; set; }
        public string SourceWalletId { get; set; }
        public string WalletId { get; set; }
    }

    public class OperationCreateReturnDTO : OperationCreateDTO
    {
        public OperationCreateReturnDTO() { }
        public OperationCreateReturnDTO(OperationCreateDTO model)
        {
            AssetId = model.AssetId;
            Amount = model.Amount;
            SourceWalletId = model.SourceWalletId;
            WalletId = model.WalletId;
        }

        public string Id { get; set; }
    }
}
