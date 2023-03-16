using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Configuration;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService
{
    public class ProgramUserAccount
    {
        public static void Main(string[] args) 
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            // Adding authentication and the extended ConfigureIdentity method.
            builder.Services.AddAuthentication();
            builder.Services.ConfigureIdentity();


            // Fetches the appsettings.json file to inject into a context.
            // Encrypt=False is a quick fix for a reintroduced bug in SQL Server 2022 - see https://stackoverflow.com/a/70850834 for more information.
            builder.Configuration.AddJsonFile("appsettings.json");


            // Development purposes only! Those with Windows can use Microsoft SQL Server and those with mac can use MySQL.

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var connectionString = builder.Configuration
                    .GetConnectionString("SqlServerConnectionString");

                builder.Services.AddDbContext<LoginDbContext>(options => options.UseSqlServer(connectionString));
                builder.Services.AddDbContext<UserAccountContext>(options => options.UseSqlServer(connectionString));
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) 
            {
                // Connection string for MySQL-database (only for stian)
                var connectionString = builder.Configuration.
                    GetConnectionString("MySqlConnectionString");
                
                builder.Services.AddDbContext<LoginDbContext>(options => options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 32)),
                    mysqlOptions =>
                    {
                        mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    }
                ));
                builder.Services.AddDbContext<UserAccountContext>(options => options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 32)),
                    mysqlOptions =>
                    {
                        mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    }
                ));
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Development machines using Linux can do something here.
            }


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
}

