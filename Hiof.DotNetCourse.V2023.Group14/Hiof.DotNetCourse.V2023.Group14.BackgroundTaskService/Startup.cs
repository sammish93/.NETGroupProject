using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService
{
    public class Startup
    {
        public void Configure(IServiceCollection services)
        {
            // Set up Hangfire with SQL server storage
            string connectionString = "<SQL connection string>";
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString);

            // Add Hangfire service
            services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
        }

 
        public void Configure(IApplicationBuilder application)
        {
            application.UseHangfireDashboard();

        }
    }
}

