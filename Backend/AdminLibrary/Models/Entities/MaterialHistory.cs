using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class MaterialHistory : EntityBase<int>
    {
        public MaterialHistory()
        {
            
        }
        public MaterialHistory(int materialId, int userId, string? observations, string movementType, DateTime? movementDate)
        {
            MaterialsId = materialId;
            UserId = userId;
            Observations = observations;
            MovementType = movementType;
            MovementDate = movementDate;
        }

        public int MaterialsId { get; set; }
        public int UserId { get; set; }
        public string? Observations { get; set; }
        public string MovementType { get; set; } 
        public DateTime? MovementDate { get; set; }
        public virtual MaterialsModel MaterialsVirtual { get; set; } = null!;
        public virtual Users UserVirtual { get; set; } = null!;
        
    }
}
