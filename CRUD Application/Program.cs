using CRUD_Application.Data;
using CRUD_Application.Handlers.Extensions;
using CRUD_Application.Repositories;
using CRUD_Application.Repositories.CollegeManagement.Repositories;
using CRUD_Application.Repositories.Interface;
using CRUD_Application.Scaffolded.Data;
using CRUD_Application.Services;
using CRUD_Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("DefaultConnection")));

// Scaffolde Database
builder.Services.AddDbContext<CollegeDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IStudentRepository,StudentRepository>();
builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();

// Services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IDepartmentService,DepartmentService>();

// Authorization
builder.Services.AddAuthorization();

// Global Validation Response

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value!.Errors
                    .Select(e =>
                        string.IsNullOrWhiteSpace(e.ErrorMessage)
                            ? "Invalid value."
                            : e.ErrorMessage)
                    .ToArray()
            );

        var response = new
        {
            success = false,
            message = "Validation failed.",
            data = (object?)null,
            errors = errors,
            traceId = context.HttpContext.TraceIdentifier
        };

        return new BadRequestObjectResult(response);
    };
});

var app = builder.Build();

app.UseGlobalExceptionHandler();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();