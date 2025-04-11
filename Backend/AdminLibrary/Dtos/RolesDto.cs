namespace AdminLibrary.Dtos
{
    public class RolesDto(
        int id,
        string name,
        string? description,
        bool status = false
        )
    {
        public int Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;  
        public bool Status { get; set; } =status;
    }
}
