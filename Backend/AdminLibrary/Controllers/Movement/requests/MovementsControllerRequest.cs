using System.Text.Json.Serialization;

namespace AdminLibrary.Controllers.Movement.requests
{
    public class MovementsControllerRequest
    {
        [JsonPropertyName("movementType")]
        public string? MovementType { get; set; }
        [JsonPropertyName("identifier")]
        public string? Identifier { get; set; }
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }
        [JsonPropertyName("observations")]
        public string? Observations { get; set; }
    }
}
