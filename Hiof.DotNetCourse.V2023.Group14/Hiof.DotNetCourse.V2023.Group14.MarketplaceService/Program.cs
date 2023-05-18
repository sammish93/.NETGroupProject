using System.Runtime.InteropServices;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1;
using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Interfaces.V1;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Data;
using Hiof.DotNetCourse.V2023.Group14.MarketplaceService.Services;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


namespace Hiof.DotNetCourse.V2023.Group14.MarketplaceService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<V1MarketplaceService>();
        builder.Services.AddControllers();
        builder.Configuration.AddJsonFile("appsettings.json");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

            builder.Services.AddDbContext<MarketplaceContext>(options => options.UseSqlServer(connectionString));
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            // Connection string for MySQL-database (only for stian)
            var connectionString = builder.Configuration.
                GetConnectionString("MySqlServerConnection");

            builder.Services.AddDbContext<MarketplaceContext>(options => options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 32)),
                mysqlOptions =>
                {
                    mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                }
            ));
        }

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}



