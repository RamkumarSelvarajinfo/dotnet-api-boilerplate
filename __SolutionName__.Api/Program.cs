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
    // JWT setup...
});
builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.RegisterApplication();
builder.Services.RegisterInfra(builder.Configuration);

// Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "__SolutionName__ API v1");
        options.RoutePrefix = string.Empty;
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
app.UseResponseCompression();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();