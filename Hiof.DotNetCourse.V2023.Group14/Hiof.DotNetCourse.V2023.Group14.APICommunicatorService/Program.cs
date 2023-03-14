using Hiof.DotNetCourse.V2023.Group14.APICommunicatorService.Configuration;

namespace Hiof.DotNetCourse.V2023.Group14.APICommunicatorService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ApiCommunicatorSettings>(builder.Configuration.GetSection("ApiCommunicator"));

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline. Running this program should open Swagger in your browser at https://localhost:<portNumber>/swagger/index.html.
            // If it doesn't work, it could be that you haven't selected the solution before running. It could also be blocked by an anti-virus.
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


