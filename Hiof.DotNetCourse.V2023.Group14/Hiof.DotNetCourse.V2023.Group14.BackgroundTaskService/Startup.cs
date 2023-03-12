using System;
using Hangfire;
using Hangfire.MySql;
using Hangfire.AspNetCore;
using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.BackgroundJobs;
using Microsoft.Extensions.Configuration;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Hangfire.SqlServer;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var connection = "server=localhost;database=background_task;Integrated Security=True";

                services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connection, new SqlServerStorageOptions
                        {
                            QueuePollInterval = TimeSpan.FromSeconds(10),
                            JobExpirationCheckInterval = TimeSpan.FromHours(1),
                            CountersAggregateInterval = TimeSpan.FromMinutes(5),
                            PrepareSchemaIfNecessary = true,
                            DashboardJobListLimit = 23000,
                            TransactionTimeout = TimeSpan.FromMinutes(1)
                        }));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var connection = "server=localhost;database=background_task;uid=root;Allow User Variables=true";

                services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(
                        new MySqlStorage(connection, new MySqlStorageOptions
                        {
                            QueuePollInterval = TimeSpan.FromSeconds(10),
                            JobExpirationCheckInterval = TimeSpan.FromHours(1),
                            CountersAggregateInterval = TimeSpan.FromMinutes(5),
                            PrepareSchemaIfNecessary = true,
                            DashboardJobListLimit = 23000,
                            TransactionTimeout = TimeSpan.FromMinutes(1),
                            TablesPrefix = "Hangfire",
                        }
                        )
                    ));
            }


            // Add the processing server as IHostedService
            services.AddHangfireServer(options => options.WorkerCount = 1);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            // Use the Hangfire Dashboard with the new UI.
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                IsReadOnlyFunc = (DashboardContext context) => true
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}


