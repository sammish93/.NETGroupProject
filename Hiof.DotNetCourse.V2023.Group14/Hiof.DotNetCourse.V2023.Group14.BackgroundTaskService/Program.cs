using Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService.Controllers;
using System.Net.Http;
using Hangfire;
using System.Xml.Linq;

namespace Hiof.DotNetCourse.V2023.Group14.BackgroundTaskService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            // Add services

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            startup.Configure(app, builder.Environment);
        }
    }

}





