namespace AdminLibrary.Dtos
{
    public class UsersDto(
        string firtsName,
        string? middleName,
        string firtsLastName,
        string? secondLastName,
        string typeIdentification,
        string numberIdentification,
        bool status ,
        string? userName,
        string userType
        )
    {
        public string FirtsName { get; set; } = firtsName;
        public string? MiddleName { get; set; } = middleName;
        public string FirtsLastName { get; set; } = firtsLastName;
        public string? SecondLastName { get; set; } = secondLastName;
        public string TypeIdentification { get; set; } = typeIdentification;
        public string NumberIdentification { get; set; } = numberIdentification;
        public bool Status { get; set; } = status;  
        public string? UserName { get; set; } = userName;   
        public string UserType { get; set; } = userType;
    }
}
