using System.Text.Json.Serialization;

namespace AdminLibrary.Controllers.Movement.requests
{
    public class MovementsControllerRequest
    {
        [JsonPropertyName("movementType")]
        public string? MovementType { get; set; }
        [JsonPropertyName("materialId")]
        public int MaterialId { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("observations")]
        public string? Observations { get; set; }
    }
}
