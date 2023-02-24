using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService
{
    public class Startup
    {
        public void Configure(IServiceCollection services)
        {
            var dbHost = "localhost";
            var dbName = "backrgound_task";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Set up Hangfire with SQL server storage
                string connection = $"Server = {dbHost};Database = {dbName};Trusted_Connection = Yes;Encrypt=False;";
                GlobalConfiguration.Configuration.UseSqlServerStorage(connection);

                // Add Hangfire service
                services.AddHangfire(x => x.UseSqlServerStorage(connection));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
                var connection = $"Server={dbHost};Database={dbName};Uid=root;Password={password}";
                GlobalConfiguration.Configuration.UseSqlServerStorage(connection);

                // Add Hangfire service
                services.AddHangfire(x => x.UseSqlServerStorage(connection));

            }
        }

 
        public void Configure(IApplicationBuilder application)
        {
            application.UseHangfireDashboard();

        }
    }
}

