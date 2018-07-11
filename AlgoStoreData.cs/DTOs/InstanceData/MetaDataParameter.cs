namespace AlgoStoreData.DTOs.InstanceData
{
    public class MetaDataParameter
    {
        public string Key { get; set; }
        public dynamic Value { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }

        public MetaDataParameter() { }

        public MetaDataParameter(string key, dynamic value, string type)
        {
            Key = key;
            Value = value;
            Description = null;
            Type = type;
        }

        public MetaDataParameter(string key, dynamic value, string description, string type)
        {
            Key = key;
            Value = value;
            Description = description;
            Type = type;
        }
    }
}
