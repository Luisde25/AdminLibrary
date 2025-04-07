using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class MaterialsConfig : IEntityTypeConfiguration<MaterialsModel>
    {
        public void Configure(EntityTypeBuilder<MaterialsModel> builder)
        {
            builder.ToTable("MATERIALS", "masters");


            builder.Property(x => x.Id)
                 .HasColumnType("int")
                 .HasColumnName("ID_MATERIALS");

            builder.Property(x => x.Identifier)
                .HasColumnType("varchar")
                .HasColumnName("IDENTIFIER");

            builder.Property(x => x.Title)
             .HasColumnType("varchar")
             .HasColumnName("TITLE");

            builder.Property(x => x.RegisterDate)
                .HasColumnType("datetime")
                 .HasColumnName("REGISTER_DATE");

            builder.Property(x => x.RegisterQuantity)
                .HasColumnType("int")
                .HasColumnName("REGISTER_QUANTITY");

            builder.Property(x => x.CurrentQuantity)
                .HasColumnType("int")
                .HasColumnName("CURRENT_QUANTITY");

            Auditory(builder);
        }

        private static void Auditory(EntityTypeBuilder<MaterialsModel> builder)
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
