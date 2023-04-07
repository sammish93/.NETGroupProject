
using Hiof.DotNetCourse.V2023.Group14.MessagingService.Services;

namespace Hiof.DotNetCourse.V2023.Group14.MessagingService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddScoped<V1MessagingService>();
        builder.Services.AddControllers();
        builder.Configuration.AddJsonFile("appsettings.json");

        builder.Services.AddControllers();
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



