using __SolutionName__.Application.Interfaces;
using __SolutionName__.Application.Services;
using __SolutionName__.Application.Validators.Flights;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace __SolutionName__.Application
{
    public static class IOC
    {
        public static void RegisterApplication(this IServiceCollection services)
        {
            services.AddMapping();
            services.AddValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<IFlightService, FlightService>();
        }

        public static void AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static void AddValidation(this IServiceCollection services)
        {
            services.AddScoped<CreateFlightValidator>();
        }
    }
}
