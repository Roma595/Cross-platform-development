using Microsoft.EntityFrameworkCore;
using KotkovAPI.Data;
using KotkovAPI.Data.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<KotkovAPIContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
// Add services to the container.

builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<AttendenceService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
