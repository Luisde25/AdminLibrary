using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class Users : EntityBase<int>
    {
        public Users()
        {
            
        }
        public Users(
            string firtsName,
            string? middleName,
            string firtsLastName,
            string secondLastName,
            string typeIdentification,
            string numberIdentification,
            bool status,
            string? userName,
            string userType
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

        /// <summary>
        /// Relacion de muchos a Uno.
        /// </summary>
        public virtual ICollection<UsersRoles> UsersRolesVirtual { get; set; } = null!;
        public virtual ICollection<MaterialsMovements> MovementsVirtual { get; set; } = null!;
    }
}
