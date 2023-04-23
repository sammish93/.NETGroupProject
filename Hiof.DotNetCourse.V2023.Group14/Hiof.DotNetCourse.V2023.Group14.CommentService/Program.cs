using System.Runtime.InteropServices;
using Hiof.DotNetCourse.V2023.Group14.CommentService.Repositories;
using Hiof.DotNetCourse.V2023.Group14.CommentService.Services;
using Hiof.DotNetCourse.V2023.Group14.CommentService.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    var connectionString = builder.Configuration
        .GetConnectionString("SqlServerConnectionString");

    builder.Services.AddDbContext<CommentServiceContext>(options => options.UseSqlServer(connectionString));
}
else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    // Connection string for MySQL-database (only for stian).
    var connectionString = builder.Configuration
        .GetConnectionString("MySqlConnectionString");

    builder.Services.AddDbContext<CommentServiceContext>(options => options.UseMySql(
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

builder.Services.AddGrpc().AddJsonTranscoding();

builder.Services.AddGrpcSwagger().AddSwaggerGen();



var app = builder.Build();
app.UseSwagger().UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "CommentService"); });

// Configure the HTTP request pipeline.

app.MapGrpcService<CommentService>();
//app.MapGrpcService<OrderService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
