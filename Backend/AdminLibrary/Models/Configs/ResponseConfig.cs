using AdminLibrary.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class ResponseConfig : IEntityTypeConfiguration<Response>
    {
        public void Configure(EntityTypeBuilder<Response> builder)
        {
            builder.ToTable("RESPONSE", "adm");

            builder.Property(x => x.Id)
                 .HasColumnType("int")
                 .HasColumnName("ID_RESPONSE");

            builder.Property(x => x.Code)
                .HasColumnType("varchar")
                .HasColumnName("RESPONSE_CODE");

            builder.Property(x => x.Message)
                .HasColumnType("varchar")
                .HasColumnName("RESPONSE_MESSAGE");

            builder.Property(x => x.Status)
                .HasColumnType("varchar")
                .HasColumnName("RESPONSE_STATUS");

            Auditory(builder);

        }

        private static void Auditory(EntityTypeBuilder<Response> builder)
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
