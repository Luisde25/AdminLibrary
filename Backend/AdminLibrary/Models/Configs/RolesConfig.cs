using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class RolesConfig : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("ROLES", "adm");

            builder.Property(x => x.Id)
                .HasColumnType("int")
                .HasColumnName("ID_ROL");

            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasColumnName("ROL_NAME");

            builder.Property(x => x.Description)
                .HasColumnType("varchar")
                .HasColumnName("DESCRIPTION");

            builder.Property(x => x.Status)
             .HasColumnType("bit")
             .HasColumnName("STATUS");

            Auditory(builder);
        }

        private static void Auditory(EntityTypeBuilder<Roles> builder)
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
