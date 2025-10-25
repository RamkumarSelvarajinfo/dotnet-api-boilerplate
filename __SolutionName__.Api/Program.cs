using __SolutionName__.Api.Extensions;
using __SolutionName__.Api.Filters;
using __SolutionName__.Api.Middlewares;
using __SolutionName__.Application;
using __SolutionName__.Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>(); // Global Exception Handler
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Flight Booking API",
        Version = "v1",
        Description = "API documentation for Flight Booking System",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Support Team",
            Email = "support@__SolutionName__.com"
        }
    });

    // Enable Swagger annotations (optional)
    options.EnableAnnotations();

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.RegisterApplication();
builder.Services.RegisterInfra(builder.Configuration);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Flight Booking API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseCors(x => {
    x.AllowAnyHeader();
    x.AllowAnyOrigin();
    x.AllowAnyMethod();
});

app.UseMiddleware<ValidationMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();