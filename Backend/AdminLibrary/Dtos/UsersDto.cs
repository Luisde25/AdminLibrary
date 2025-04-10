namespace AdminLibrary.Dtos
{
    public class UsersDto
    {

        public UsersDto()
        {
            
        }

        public UsersDto(
        string firtsName,
        string? middleName,
        string firtsLastName,
        string? secondLastName,
        string typeIdentification,
        string numberIdentification,
        bool status,
        string? userName,
        string userType,
        string? rolName
        )
        {
            FirtsName = firtsName;
            MiddleName = middleName;
            FirtsLastName = firtsLastName;
            SecondLastName = secondLastName;
            TypeIdentification = typeIdentification;
            NumberIdentification = numberIdentification;
            Status = status;
            UserName = userName;
            UserType = userType;
            RolName = rolName ?? string.Empty;
        }

        public string FirtsName { get; set; }
        public string? MiddleName { get; set; } 
        public string FirtsLastName { get; set; } 
        public string? SecondLastName { get; set; }
        public string TypeIdentification { get; set; } 
        public string NumberIdentification { get; set; } 
        public bool Status { get; set; } 
        public string? UserName { get; set; }    
        public string UserType { get; set; }
        public string RolName { get; set; } 
    }
}
