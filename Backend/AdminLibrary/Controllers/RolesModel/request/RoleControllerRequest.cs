using System.Text.Json.Serialization;

namespace AdminLibrary.Controllers.RolesModel.request
{
    public class RoleControllerRequest
    {

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public bool status { get; set; } = true;
    }

    public class RoleControllerRequestUpdate
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } 
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public bool status { get; set; }
    }
}
