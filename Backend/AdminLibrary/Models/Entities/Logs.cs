using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class Logs: EntityBase<int>
    {
        public string? LogSource { get; set; }
        public string? LogTable { get; set; }
        public string? LogAction { get; set; }
        public string? LogMsn { get; set; }
    }
}
