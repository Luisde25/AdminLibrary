namespace AdminLibrary.Dtos
{
    public class MenusDto(
        string name, 
        string? url,
        string? father,
        int? order,
        bool status = false
        )
    {
        public string Name { get; set; } = name;
        public string? Url { get; set; } = url;
        public string? Father { get; set; } = father;
        public int? Order { get; set; } = order;
        public bool Status { get; set; } = status;
    }
}
