using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminLibrary.Models.Configs
{
    public class UsersConfig : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("USERS", "adm");

            builder.Property(x => x.Id)
                .HasColumnType("int")
                .HasColumnName("ID_USER");

            builder.Property(x => x.FirtsName)
               .HasColumnType("varchar")
               .HasColumnName("FIRST_NAME");

            builder.Property(x => x.MiddleName)
              .HasColumnType("varchar")
              .HasColumnName("MIDDLE_NAME");

            builder.Property(x => x.FirtsLastName)
              .HasColumnType("varchar")
              .HasColumnName("LAST_NAME");

            builder.Property(x => x.SecondLastName)
              .HasColumnType("varchar")
              .HasColumnName("SECOND_LAST_NAME");

            builder.Property(x => x.TypeIdentification)
             .HasColumnType("varchar")
             .HasColumnName("TYPE_IDENTIFICATION");

            builder.Property(x => x.NumberIdentification)
               .HasColumnType("varchar")
               .HasColumnName("NUMBER_IDENTIFICATION");

            builder.Property(x => x.Status)
                 .HasColumnType("bit")
                 .HasColumnName("STATUS");

            builder.Property(x => x.UserName)
               .HasColumnType("varchar")
               .HasColumnName("USER_NAME");

            builder.Property(x => x.UserType)
              .HasColumnType("varchar")
              .HasColumnName("USER_TYPE");

            Auditory(builder);
        }

        private static void Auditory(EntityTypeBuilder<Users> builder)
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
