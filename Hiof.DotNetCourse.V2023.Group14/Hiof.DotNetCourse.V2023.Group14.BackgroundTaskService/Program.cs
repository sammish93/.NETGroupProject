using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers;
using System.Net.Http;
using Hangfire;
using System.Xml.Linq;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Data;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.DTO.V1;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json");
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            // Add services
            builder.Services.AddScoped<MessageChecker>();
            builder.Services.AddSignalR();
            builder.Services.AddControllers();


            var dbHost = "localhost";
            var dbName = "user_accounts";
            var dbConnectionStr = $"Server = {dbHost};Database = {dbName};Trusted_Connection = Yes;Encrypt=False;";
            var msgConnectionStr = $"Server = {dbHost};Database = user_messages;Trusted_Connection = Yes;Encrypt=False;";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                builder.Services.AddDbContext<LoginDbContext>(options => options.UseSqlServer(dbConnectionStr));
                builder.Services.AddDbContext<UserAccountContext>(options => options.UseSqlServer(dbConnectionStr));
                builder.Services.AddDbContext<MessagingContext>(options => options.UseSqlServer(msgConnectionStr));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                
                var connectionStr = $"Server={dbHost};Database={dbName};Uid=root;";
                var mConnectionStr = $"Server={dbHost};Database=user_messages;Uid=root;";
                builder.Services.AddDbContext<LoginDbContext>(options => options.UseMySql(
                    connectionStr,
                    new MySqlServerVersion(new Version(8, 0, 32)),
                    mysqlOptions =>
                    {
                        mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    }
                ));
                builder.Services.AddDbContext<UserAccountContext>(options => options.UseMySql(
                    connectionStr,
                    new MySqlServerVersion(new Version(8, 0, 32)),
                    mysqlOptions =>
                    {
                        mysqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore);
                    }
                ));
                builder.Services.AddDbContext<MessagingContext>(options => options.UseMySql(
                    mConnectionStr,
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

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials());
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.MapHub<MessageHub>("/MessageHub");
            app.UseCors("CorsPolicy");

            startup.Configure(app, builder.Environment);
        }
    }

}





