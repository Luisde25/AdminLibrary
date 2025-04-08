using AdminLibrary.Models.Shared;

namespace AdminLibrary.Models.Entities
{
    public class Menus: EntityBase<int>
    {
        public Menus()
        {
            
        }
        public Menus(
            string name,
            string? url,
            string? father,
            int order,
            bool status
            )
        {
            Name = name; 
            Url = url;
            Father = father;
            Order = order;
            Status = status;
        }
        public string Name { get; set; } 
        public string? Url { get; set; }
        public string? Father { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<RolesMenus> MenusRolesVirtual { get; set; } = null!;
    }
}
