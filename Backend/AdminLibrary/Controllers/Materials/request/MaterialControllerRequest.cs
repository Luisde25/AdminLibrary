using System.Text.Json.Serialization;

namespace AdminLibrary.Controllers.Materials.request
{
    public class MaterialControllerRequest
    {
        [JsonPropertyName("Referencia")]
        public string Identifier { get; set; } = string.Empty;
        [JsonPropertyName("Titulo")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("Cantidad Registrada")]
        public int RegisterQuantity { get; set; }
    }

    public class MaterialControllerUpdate
    {
        [JsonPropertyName("Referencia")]
        public string Identifier { get; set; } = string.Empty;
        [JsonPropertyName("Titulo")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("Cantidad Registrada")]
        public int RegisterQuantity { get; set; }

        [JsonPropertyName("Cantidad Actual")]
        public int CurrentQuantity { get; set; }
    }

}
