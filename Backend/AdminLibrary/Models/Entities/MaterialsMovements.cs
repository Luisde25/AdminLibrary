using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class MaterialsMovements : EntityBase<int>
    {
        public int MaterialsId { get; set; }
        public int UserId { get; set; }
        public string? Observations { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public DateTime? MovementDate { get; set; }
        public virtual MaterialsModel MaterialsVirtual { get; set; } = null!;
        public virtual Users UserVirtual { get; set; } = null!;
      
    }
}
