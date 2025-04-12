using AdminLibrary.Shared;

namespace AdminLibrary.Models.Entities
{
    public class Response : EntityBase<int>
    {
        public Response() { }
        public Response(
            string code,
            string message,
            string status)
        {
            Code = code;
            Message = message;
            Status = status;
        }
        public string? Code { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }

    }
}
