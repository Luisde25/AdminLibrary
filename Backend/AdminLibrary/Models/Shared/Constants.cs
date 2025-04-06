namespace AdminLibrary.Models.Shared
{
    public class Constants
    {
        public static readonly DateTime utcNow = DateTime.UtcNow;
        public static readonly TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
    }

    public static class Codes
    {
        public static string success = "SUC-VA-0001";
        public static string failed = "ERR-VA-0002";
        public static string requestInvalid = "ERR-VA-0003";
        public static string updateFailed = "ERR-VA-0004";
        public static string exists = "ERR-VA-0005";
    }
}
