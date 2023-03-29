
using Hiof.DotNetCourse.V2023.Group14.ReadingGoalService.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Runtime.InteropServices;

namespace Hiof.DotNetCourse.V2023.Group14.ReadingGoalService
{
    public class ProgramReadingGoals
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json");


            // Development purposes only! Those with Windows can use Microsoft SQL Server and those with mac can use MySQL.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var connectionString = builder.Configuration
                    .GetConnectionString("SqlServerConnectionString");

                builder.Services.AddDbContext<ReadingGoalsContext>(options => options.UseSqlServer(connectionString));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // Connection string for MySQL-database (only for stian).
                var connectionString = builder.Configuration
                    .GetConnectionString("MySqlConnectionString");

                builder.Services.AddDbContext<ReadingGoalsContext>(options => options.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 32)),
                    mysqlOptions =>
                    {
                        mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    }
                ));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // Development machines using Linux can do something here.
            }

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

