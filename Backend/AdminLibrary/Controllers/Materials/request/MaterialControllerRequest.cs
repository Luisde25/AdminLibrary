using System.Text.Json.Serialization;

namespace AdminLibrary.Controllers.Materials.request
{
    public class MaterialControllerRequest
    {
        [JsonPropertyName("references")]
        public string Identifier { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("register quantity")]
        public int RegisterQuantity { get; set; }

        [JsonPropertyName("observations")]
        public string? Observacion { get; set; }
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
    }

    public class MaterialControllerUpdate
    {
        [JsonPropertyName("references")]
        public string Identifier { get; set; } = string.Empty;
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("register quantity")]
        public int RegisterQuantity { get; set; }
        [JsonPropertyName("current quantity")]
        public int CurrentQuantity { get; set; }

        [JsonPropertyName("observations")]
        public string? Observacion { get; set; }
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }

        
    }

}
