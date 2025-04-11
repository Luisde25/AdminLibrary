namespace AdminLibrary.Dtos
{
    public class MaterialDto(
        int id,
        string identifier,
        string title,
        string registerDate,
        int registerQuantity,
        int currentQuantity)
    {
        public int Id { get; set; } = id;
        public string? Identifier { get; set; } = identifier;
        public string? Title { get; set; } = title;
        public string? RegisterDate { get; set; } = registerDate;
        public int? RegisterQuantity { get; set; } = registerQuantity;
        public int? CurrentQuantity { get; set; } = currentQuantity;
    }
}
