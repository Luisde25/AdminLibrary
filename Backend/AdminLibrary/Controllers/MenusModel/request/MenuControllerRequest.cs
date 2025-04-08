using System.Text.Json.Serialization;

namespace AdminLibrary.Controllers.MenusModel.request
{
    public class MenuControllerRequest
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("father")]
        public string? Father { get; set; }
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; } = true;
    }

    public class MenuControllerUpdate
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("father")]
        public string? Father { get; set; }
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; } = true;
    }
}
