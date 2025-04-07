using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class LogsConfig : IEntityTypeConfiguration<Logs>
    {
        public void Configure(EntityTypeBuilder<Logs> builder)
        {
            builder.ToTable("LOG_TRANSACTION", "adm");

            builder.Property(x => x.Id)
                .HasColumnType("int")
                .HasColumnName("ID_LOG");

            builder.Property(x => x.LogSource)
              .HasColumnType("varchar")
              .HasColumnName("LOG_SOURCE");

            builder.Property(x => x.LogTable)
             .HasColumnType("varchar")
             .HasColumnName("LOG_TABLE");

            builder.Property(x => x.LogAction)
             .HasColumnType("varchar")
             .HasColumnName("LOG_ACTION");

            builder.Property(x => x.LogMsn)
             .HasColumnType("varchar")
             .HasColumnName("LOG_MSN");

            Auditory(builder);
        }

        private static void Auditory(EntityTypeBuilder<Logs> builder)
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
