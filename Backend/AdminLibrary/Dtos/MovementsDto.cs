namespace AdminLibrary.Dtos
{
    public class MovementsDto(
        string materialId,
        string userId,
        string? observations,
        string movementType,
        DateTime? movementDate
        )
    {
        public string Materials { get; set; } = materialId;
        public string User { get; set; } = userId;
        public string? Observations { get; set; } = observations;
        public string MovementType { get; set; }  = movementType;
        public DateTime? MovementDate { get; set; } = movementDate;
    }
}
