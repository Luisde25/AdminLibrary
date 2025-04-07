using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class Menus: EntityBase<int>
    {
        public string Name { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Father { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<RolesMenus> MenusRolesVirtual { get; set; } = null!;
    }
}
