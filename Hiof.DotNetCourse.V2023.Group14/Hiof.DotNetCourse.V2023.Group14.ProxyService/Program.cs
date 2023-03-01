using Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var apiUrls = builder.Configuration
    .GetSection("V1UserAccountApiUrls")
    .Get<V1UserAccountApiUrls>() ?? new V1UserAccountApiUrls();

builder.Services.AddSingleton(apiUrls);

// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


