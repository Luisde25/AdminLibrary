using AdminLibrary.Dtos;
using AdminLibrary.Shared;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<MovementsDto>> HistoryMovements(AppDbContext context)
        {
            var listMovements = await (from m in context.History
                                       join u in context.Users on m.UserId equals u.Id
                                       join mt in context.Materials on m.MaterialsId equals mt.Id
                                       select new MovementsDto(
                                           mt.Title,
                                           u.UserName!,
                                           m.Observations,
                                           m.MovementType,
                                           m.MovementDate
                                       )).ToListAsync();

            if (listMovements.Count == 0)
            {
                return [];
            }

            return listMovements;
        }
    }
}
