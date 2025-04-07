using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class MenusConfig : IEntityTypeConfiguration<Menus>
    {
        public void Configure(EntityTypeBuilder<Menus> builder)
        {
            builder.ToTable("MENUS", "adm");

            builder.Property(x => x.Id)
                .HasColumnType("int")
                .HasColumnName("ID_MENU");

            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasColumnName("MENU_NAME");

            builder.Property(x => x.Url)
            .HasColumnType("varchar")
            .HasColumnName("MENU_URL");

            builder.Property(x => x.Father)
           .HasColumnType("varchar")
           .HasColumnName("MENU_FATHER");

            builder.Property(x => x.Order)
            .HasColumnType("int")
            .HasColumnName("MENU_ORDER");

            builder.Property(x => x.Status)
            .HasColumnType("bit")
            .HasColumnName("STATUS");

            Auditory(builder);
        }
        private static void Auditory(EntityTypeBuilder<Menus> builder)
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
