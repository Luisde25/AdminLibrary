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
        public static string MaterialNoFound = "ERR-VA-0006"; 
        public static string UserNoFound = "ERR-VA-0007";
        public static string UserNoLoans = "ERR-VA-0008";
        public static string MaxEstudents = "ERR-VA-0009";
        public static string MaxProf = "ERR-VA-0010";
        public static string MaxAdmin = "ERR-VA-0011";
        public static string LimitMaterial = "ERR-VA-0012";
        public static string DeleteSuccess = "ERR-VA-0013";
        public static string UserNoDelete = "ERR-VA-0014";
        
    }
}
