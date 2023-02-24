using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService
{
    public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var dbHost = "localhost";
        var dbName = "background_task";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Set up Hangfire with SQL server storage
            string connection = $"Server={dbHost};Database={dbName};Trusted_Connection=True;";

            services.AddHangfire(x => x.UseSqlServerStorage(connection));
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            string connection = $"Server={dbHost};Database={dbName};Uid=root;Password={password}";
            string tablePrefix = "backgroundJob_";

            services.AddHangfire(x =>
            {
                x.UseStorage(
                    new MySqlStorage(connection,
                    new MySqlStorageOptions { TablesPrefix = tablePrefix }));
            });
        }
    }

    public void Configure(IApplicationBuilder application)
    {
        application.UseHangfireDashboard();
    }
}

}

