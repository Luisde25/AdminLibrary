using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AdminLibrary.Models.Configs
{
    public class MaterialHistoryConfig : IEntityTypeConfiguration<MaterialHistory>
    {
        public void Configure(EntityTypeBuilder<MaterialHistory> builder)
        {
            builder.ToTable("MATERIALS_HISTORY", "masters");

            builder.HasOne(x => x.MaterialsVirtual).WithMany(y => y.HistoryVirtual)
                .HasForeignKey(x => x.MaterialsId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.UserVirtual).WithMany(y => y.HistoryVirtual)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
                .HasColumnType("int")
                .HasColumnName("ID_HISTORY");

            builder.Property(x => x.MaterialsId)
                .HasColumnType("int")
                .HasColumnName("ID_MATERIAL");

            builder.Property(x => x.UserId)
                .HasColumnType("int")
                .HasColumnName("ID_USER");

            builder.Property(x => x.Observations)
                .HasColumnType("varchar")
                .HasColumnName("OBSERVATIONS");

            builder.Property(x => x.MovementType)
                .HasColumnType("varchar")
                .HasColumnName("MOVEMENT_TYPE");

            builder.Property(x => x.MovementDate)
                .HasColumnType("datetime")
                .HasColumnName("MOVEMENT_DATE");

            Auditory(builder);
        }

        private static void Auditory(EntityTypeBuilder<MaterialHistory> builder)
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
