using AdminLibrary.Shared;

namespace AdminLibrary.Models.Entities
{
    public class UsersRoles : EntityBase<int>
    {
        public UsersRoles()
        {
            
        }
        public int IdUser { get; set; }
        public int IdRol { get; set; }
        public bool Status { get; set; }

        public virtual Roles RolesVirtual { get; set; } = null!;
        public virtual Users UserlesVitrual { get; set; } = null!;  
    }
}
