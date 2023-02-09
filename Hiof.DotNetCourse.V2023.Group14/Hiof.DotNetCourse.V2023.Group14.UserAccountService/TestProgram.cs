/*
using Hiof.DotNetCourse.V2023.Group14.UserAccountService.Data;
using Microsoft.EntityFrameworkCore;

namespace Hiof.DotNetCourse.V2023.Group14.UserAccountService
{
public class TestProgram
  {
      public static void Main(string[] args)
      {
          var builder = WebApplication.CreateBuilder(args);

          // Add services to the container.

          builder.Services.AddControllers();

          // Right now the database connection is hardcoded. This is bad for security. We can place the dbHost, dbName, and dbPwd in our secrets
          // in the pipeline and inject them at that stage. Doing it this way just makes it easier in the early stages of development.
          var dbHost = "Server=tcp:dotnetgroupproject.database.windows.net,1433";
          var dbName = "DotNetGroupProjectSQLDB";
          var dbPwd = "Hodgeheg14";
          var dbConnectionStr = $"{dbHost};Initial Catalog={dbName};Persist Security Info=False;User ID=hedgehogfans;Password={dbPwd};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

          // Using dependency injection to supply our remote database (azure SQL server) to the API. This allows any Http request (GET, POST, etc) that we program
          // to run transactions with our specific database.
          // NOTE: This line needs to be before the   builder.Build();    line for it to work.
          builder.Services.AddDbContext<DbOrmTestClassContext>(options => options.UseSqlServer(dbConnectionStr));



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
*/