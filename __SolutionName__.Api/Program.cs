using __SolutionName__.Api.Extensions;
using __SolutionName__.Api.Filters;
using __SolutionName__.Api.Middlewares;
using __SolutionName__.Application;
using __SolutionName__.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "__SolutionName__ API",
        Version = "v1",
        Description = "API documentation for __SolutionName__",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Support Team",
            Email = "support@__SolutionName__.com"
        }
    });
    options.EnableAnnotations();
});

// Health & Compression
builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();

// Application + Infrastructure
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.RegisterApplication();
builder.Services.RegisterInfra(builder.Configuration);

// Secure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins("https://yourfrontend.com");
    });
});

// Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "__SolutionName__")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

// Swagger only in non-production
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "__SolutionName__ API v1");
        options.RoutePrefix = string.Empty;
    });
}

// Enforce HTTPS + HSTS in production
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseCors("DefaultCorsPolicy");

// Middlewares
app.UseMiddleware<ValidationMiddleware>();

// Only full request/response logging in dev/test
if (!app.Environment.IsProduction())
{
    app.UseMiddleware<RequestResponseLoggingMiddleware>();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseResponseCompression();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();