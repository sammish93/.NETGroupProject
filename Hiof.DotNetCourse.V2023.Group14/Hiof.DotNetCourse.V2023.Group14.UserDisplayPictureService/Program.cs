using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Data;
using Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Runtime.InteropServices;

namespace Hiof.DotNetCourse.V2023.Group14.UserDisplayPictureService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<V1UserIconService>();
            builder.Services.AddControllers();
            builder.Configuration.AddJsonFile("appsettings.json");


            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

                builder.Services.AddDbContext<UserIconContext>(options => options.UseSqlServer(connectionString));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // Connection string for MySQL-database (only for stian)
                var connectionString = builder.Configuration.
                    GetConnectionString("MySqlServerConnection");

                builder.Services.AddDbContext<UserIconContext>(options => options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 32)),
                    mysqlOptions =>
                    {
                        mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    }
                ));
            }

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            /*
            // Commented out because of a bug involving not binding a UserDisplayPictureService/Debug/bin file or whatever.
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Picture.Display.Service.Api",
                    Version = "v1",
                    Description =
                        """
                        This API has a set of HTTP endpoints that allow users
                        to manage user profile pictures. It contains endpoints
                        for retrieving user profile pictures by ID or username,
                        uploading a new profile picture, updating an existing
                        picture, and deleting a picture.
                        """

                });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyApi.xml"));
            });
            */

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                /*
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Picture.Display.Service.Api v1");
                });
                */
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


