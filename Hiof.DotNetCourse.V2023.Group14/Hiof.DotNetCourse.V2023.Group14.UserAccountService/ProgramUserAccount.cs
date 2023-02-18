
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Diagnostics.Metrics;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService
{
    public class ProgramUserAccount
    {
        public static void Main(string[] args) 
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();


            // Connection string to Azure db (cloud). The free pricing plan is too restrictive, so it's best for each of us to use a local db and seed it.
            var dbHostAzure = "Server = tcp:dotnetgroupproject.database.windows.net,1433"; 
            var dbNameAzure = "DotNetGroupProjectSQLDB";
            var dbPwdAzure = "Hodgeheg14";
            var dbConnectionStrAzure = $"{dbHostAzure};Initial Catalog={dbNameAzure};Persist Security Info=False;User ID=hedgehogfans;Password={dbPwdAzure};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            // Encrypt=False is a quick fix for a reintroduced bug in SQL Server 2022 - see https://stackoverflow.com/a/70850834 for more information.
            // IMPORTANT: We should separate each microservice in its own database, in this case it's called 'UserAccounts'.
            // The specified table is given using an Entity Framework Core annotation in the actual class (see DbOrmTestClass.cs).
            // One server can hold many databases, and one database can hold many tables.
            var dbHost = "localhost";
            var dbName = "UserAccounts";
            var dbConnectionStr = $"Server = {dbHost};Database = {dbName};Trusted_Connection = Yes;Encrypt=False;";

            builder.Services.AddDbContext<LoginDbContext>(options => options.UseSqlServer(dbConnectionStr));
            // Test Controller just for test purposes. Feel free to remove this once everyone is comfortable with the migration process.
            builder.Services.AddDbContext<DbOrmTestClassContext>(options => options.UseSqlServer(dbConnectionStr));
            builder.Services.AddDbContext<UserAccountContext>(options => options.UseSqlServer(dbConnectionStr));


            // Connection string for MySQL-database (only for stian).
            var connectionStr = "Server=localhost;Database=dotnetproject;Uid=root;Password=" + Environment.GetEnvironmentVariable("DB_PASSWORD");
            builder.Services.AddDbContext<LoginDbContext>(options => options.UseMySql(
                connectionStr,
                new MySqlServerVersion(new Version(8, 0, 32)
                )));


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

