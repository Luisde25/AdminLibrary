using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class Users : EntityBase<int>
    {
        public string FirtsName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } 
        public string FirtsLastName { get; set; } = string.Empty;
        public string? SecondLastName { get; set; }
        public string TypeIdentification { get; set; } = string.Empty;
        public string NumberIdentification { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? UserName { get; set; }
        public string UserType { get; set; } = string.Empty;

        /// <summary>
        /// Relacion de muchos a Uno.
        /// </summary>
        public virtual ICollection<UsersRoles> UsersRolesVirtual { get; set; } = null!;
        public virtual ICollection<MaterialsMovements> MovementsVirtual { get; set; } = null!;
    }
}
