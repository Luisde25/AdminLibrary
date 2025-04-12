namespace AdminLibrary.Constants
{
    public static class ConstantsApi
    {
        ///Roles
        public static readonly string Teacher = "Profesor";
        public static readonly string Student = "Estudiante";
        public static readonly string Admin = "Administrativo";
        public static readonly string User = "Usuario";

        ///Movimientos
        public static readonly string Loans = "Prestamo";
        public static readonly string Return = "Devuelto";
        public static readonly string Register = "Registro";
        public static readonly string Message = "Ingreso de un nuevo material";

        public static readonly DateTime utcNow = DateTime.UtcNow;
        public static readonly TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");

        ///Cantidad de prestamos

        public static readonly int CantStudent = 5;
        public static readonly int CantTeacher = 3;
        public static readonly int CantAdmin = 1;
    }

    public static class Codes
    {
        public static readonly string success = "SUC-VA-0001";
        public static readonly string failed = "ERR-VA-0002";
        public static readonly string requestInvalid = "ERR-VA-0003";
        public static readonly string updateFailed = "ERR-VA-0004"; 
        public static readonly string existsMaterial = "ERR-VA-0005";
        public static readonly string MaterialNoFound = "ERR-VA-0006";
        public static readonly string UserNoFound = "ERR-VA-0007";
        public static readonly string UserNoLoans = "ERR-VA-0008";
        public static readonly string MaxEstudents = "ERR-VA-0009";
        public static readonly string MaxProf = "ERR-VA-0010";
        public static readonly string MaxAdmin = "ERR-VA-0011";
        public static readonly string ExistMaterial = "ERR-VA-0012";
        public static readonly string LimitMaterial = "ERR-VA-0013";
        public static readonly string UserNoDelete = "ERR-VA-0014";
        public static readonly string DeleteSuccess = "ERR-VA-0015";
        public static readonly string existsUser = "ERR-VA-0016";
        public static readonly string existsRol = "ERR-VA-0017";
        public static readonly string MateriaReturn = "ERR-VA-0018";

    }
}
