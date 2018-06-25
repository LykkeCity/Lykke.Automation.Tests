namespace AlgoStoreData.DTOs
{
    public class CreateAlgoDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }

    public class EditAlgoDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
