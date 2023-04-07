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
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using System.Configuration;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService
{
    public class Startup
    {
        public IConfiguration configRoot { get; }
        
        public string connectionString { get; set; }

        public Startup(IConfiguration configuration)
        {

            configRoot = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var connectionString = configRoot
                    .GetConnectionString("SqlServerConnectionString");

                services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
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
                var connectionString = configRoot
                    .GetConnectionString("MySqlConnectionString");

                services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseStorage(
                        new MySqlStorage(connectionString, new MySqlStorageOptions
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
            services.AddHttpClient();
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


