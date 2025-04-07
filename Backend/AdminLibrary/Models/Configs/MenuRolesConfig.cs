using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class MenuRolesConfig : IEntityTypeConfiguration<RolesMenus>
    {
        public void Configure(EntityTypeBuilder<RolesMenus> builder)
        {
            builder.ToTable("MENU_ROLES", "adm");

            builder.HasOne(x => x.RolesVirtual).WithMany(y => y.MenusRolesVirtual)
              .HasForeignKey(x => x.RoleId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.MenusVirtual).WithMany(y => y.MenusRolesVirtual)
                .HasForeignKey(x => x.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
                .HasColumnType("int")
                .HasColumnName("ID_MENU_ROL");

            builder.Property(x => x.MenuId)
                .HasColumnType("int")
                .HasColumnName("ID_MENU");

            builder.Property(x => x.RoleId)
                .HasColumnType("int")
                .HasColumnName("ID_ROL");

            builder.Property(x => x.Status)
                .HasColumnType("bit")
                .HasColumnName("STATUS");

            Auditory(builder);
        }
        private static void Auditory(EntityTypeBuilder<RolesMenus> builder)
        {
            builder.Property(x => x.CreateDate)
               .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");

            builder.Property(x => x.CreateUser)
             .HasColumnType("varchar")
              .HasColumnName("CREATE_USER");

            builder.Property(x => x.UpdateDate)
             .HasColumnType("datetime")
              .HasColumnName("UPDATE_DATE");

            builder.Property(x => x.UpdateUser)
             .HasColumnType("varchar")
              .HasColumnName("UPDATE_USER");
        }
    }
}
