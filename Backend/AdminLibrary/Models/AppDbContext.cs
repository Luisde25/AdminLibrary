using AdminLibrary.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AdminLibrary.Models
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<MaterialsModel> Materials { get; set; }
        public DbSet<Response> Response { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            return base.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
