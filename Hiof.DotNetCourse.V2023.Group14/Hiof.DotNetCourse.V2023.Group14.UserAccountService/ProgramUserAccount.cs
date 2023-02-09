
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService
{
    public class ProgramUserAccount
    {
        public static void Main(string[] args) 
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var dbHost = "Server=tcp:dotnetgroupproject.database.windows.net,1433";
            var dbName = "DotNetGroupProjectSQLDB";
            var dbPwd = "Hodgeheg14";
            var dbConnectionStr = $"{dbHost};Initial Catalog={dbName};Persist Security Info=False;User ID=hedgehogfans;Password={dbPwd};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            builder.Services.AddDbContext<UserAccountContext>(options => options.UseSqlServer(dbConnectionStr));
            
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

