using __SolutionName__.Domain.Interfaces.Repositories;
using __SolutionName__.Infrastructure.Persistence;
using __SolutionName__.Infrastructure.Reposotories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace __SolutionName__.Infrastructure.Extensions
{
    public static class IOC
    {
        public static void RegisterInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
