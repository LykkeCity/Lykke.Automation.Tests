namespace AlgoStoreData.DTOs.InstanceData
{
    public class InstanceDataConfig
    {
        public InstanceParameters ValidMetaData { get; set; }
        public InstanceParameters InvalidInstanceAssetPair { get; set; }
        public InstanceParameters InvalidInstanceTradedAsset { get; set; }
        public InstanceParameters UseInvalidAlgoId { get; set; }
        public InstanceParameters NegativeTradeVolume { get; set; }
        public InstanceParameters ZeroTradedVolume { get; set; }
    }
}
