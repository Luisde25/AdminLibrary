using AdminLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AdminLibrary
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /// Agregar DbContext con cadena de conexión
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            /// Agregar controladores
            builder.Services.AddControllers();

            /// Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Biblioteca Digital", Version = "v1" });
            });

            ///Se oculta la consola de Lifetime
            builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.None);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Digital v1");
            });

            // CAMBIO IMPORTANTE: Mostrar Swagger si estás en modo desarrollo
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}