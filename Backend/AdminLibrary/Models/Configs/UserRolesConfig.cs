using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class UserRolesConfig : IEntityTypeConfiguration<UsersRoles>
    {
        public void Configure(EntityTypeBuilder<UsersRoles> builder)
        {
            builder.ToTable("USR_USER_ROL", "adm");

            builder.HasOne(x => x.RolesVirtual).WithMany(y => y.UsersRolesVirtual)
                .HasForeignKey(x => x.IdRol)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.UserlesVitrual).WithMany(y => y.UsersRolesVirtual)
                .HasForeignKey(x => x.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
                .HasColumnType("int")
                .HasColumnName("ID_USER_ROL");

            builder.Property(x => x.IdRol)
                  .HasColumnType("int")
                  .HasColumnName("ID_ROL");

            builder.Property(x => x.IdUser)
                .HasColumnType("int")
                .HasColumnName("ID_USER");

            builder.Property(x => x.Status)
              .HasColumnType("bit")
              .HasColumnName("STATUS");

            Auditory(builder);
        }

        private static void Auditory(EntityTypeBuilder<UsersRoles> builder)
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
