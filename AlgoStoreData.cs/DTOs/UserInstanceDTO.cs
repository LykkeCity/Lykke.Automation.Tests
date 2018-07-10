using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Enums;

namespace AlgoStoreData.DTOs
{
    public class UserInstanceDTO
    {
        public string InstanceId { get; set; }
        public string AlgoClientId { get; set; }
        public string AlgoId { get; set; }
        public string InstanceName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? RunDate { get; set; }
        public DateTime? StopDate { get; set; }
        public ClientWalletDataDTO Wallet { get; set; }
        public AlgoInstanceStatus InstanceStatus { get; set; }
        public AlgoInstanceType InstanceType { get; set; }

        public override string ToString()
        {
            return $"InstanceId: {InstanceId}; InstanceName: {InstanceName}; " +
                   $"AlgoClientId: {AlgoClientId}; AlgoId: {AlgoId}; CreateDate {CreateDate}; " +
                   $"RunDate: {RunDate}; StopDate: {StopDate}; Wallet: {Wallet};" +
                   $"InstanceStatus: {InstanceStatus}; InstanceType: {InstanceType}";
        }
    }
}
