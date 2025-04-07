namespace AdminLibrary.Dtos
{
    public class RolesDto(
        string name,
        string? description,
        bool status = false
        )
    {
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;  
        public bool Status { get; set; } =status;
    }
}
