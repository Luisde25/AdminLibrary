using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class MaterialsMovementsConfig : IEntityTypeConfiguration<MaterialsMovements>
    {
        public void Configure(EntityTypeBuilder<MaterialsMovements> builder)
        {
            builder.ToTable("MATERIALS_MOVEMENTS", "masters");

            builder.HasOne(x => x.MaterialsVirtual).WithMany(y => y.MovementsVirtual)
               .HasForeignKey(x => x.MaterialsId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.UserVirtual).WithMany(y => y.MovementsVirtual)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Id)
               .HasColumnType("int")
               .HasColumnName("ID_MOVEMENT");

            builder.Property(x => x.MaterialsId)
                .HasColumnType("int")
                .HasColumnName("ID_MATERIALS");

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

        private static void Auditory(EntityTypeBuilder<MaterialsMovements> builder)
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
