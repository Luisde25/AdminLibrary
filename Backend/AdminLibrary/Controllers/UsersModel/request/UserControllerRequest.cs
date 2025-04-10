using System.Text.Json.Serialization;

namespace AdminLibrary.Controllers.UsersModel.request
{
    public class UserControllerRequest
    {
        [JsonPropertyName("firsName")]
        public string? FirtsName { get; set; }
        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }
        [JsonPropertyName("firtsLastName")]
        public string? FirtsLastName { get; set; }
        [JsonPropertyName("secondLastName")]
        public string? SecondLastName { get; set; }
        [JsonPropertyName("typeIdentification")]
        public string? TypeIdentification { get; set; }
        [JsonPropertyName("numberIdentification")]
        public string? NumberIdentification { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; } = true;
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
        [JsonPropertyName("userType")]
        public string? UserType { get; set; }
        [JsonPropertyName("rol")]
        public int Rol { get; set; }

    }

    public class UserControllerUpdate
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("firsName")]
        public string? FirtsName { get; set; }
        [JsonPropertyName("middleName")]
        public string? MiddleName { get; set; }
        [JsonPropertyName("firtsLastName")]
        public string? FirtsLastName { get; set; }
        [JsonPropertyName("secondLastName")]
        public string? SecondLastName { get; set; }
        [JsonPropertyName("typeIdentification")]
        public string? TypeIdentification { get; set; }
        [JsonPropertyName("numberIdentification")]
        public string? NumberIdentification { get; set; }
        [JsonPropertyName("status")]
        public bool Status { get; set; } = true;
        [JsonPropertyName("userName")]
        public string? UserName { get; set; }
        [JsonPropertyName("userType")]
        public string? UserType { get; set; }
        [JsonPropertyName("rol")]
        public int Rol { get; set; }
    }
}
