using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class RolesMenus: EntityBase<int>
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public virtual Roles RolesVirtual { get; set; } = null!;
        public virtual Menus MenusVirtual { get; set; } = null!;
    }
}
