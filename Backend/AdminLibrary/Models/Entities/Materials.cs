using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class Materials : EntityBase<int>
    {
        public string Identifier { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime RegisterDate { get; set; }
        public int RegisterQuantity { get; set; } 
        public int CurrentQuantity { get; set; }

    }
}
