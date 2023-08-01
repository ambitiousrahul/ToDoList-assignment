using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrbanFTProject.Data;
using UrbanFTProject.Repository;

namespace UrbanFTProject.ToDoList.Data
{
    public static class DataConfigurations
    {
        
            public static void AddDBServices(this IServiceCollection services, IConfiguration configuration)
            {
                // Add services to the container.
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>options.UseSqlServer(connectionString));
                
                services.AddDbContext<UrbanFTAssignmentDbContext>(options =>options.UseSqlServer(connectionString));

                services.AddScoped<IRepository<TodoTask>, TodoTaskRepository>();

        }
        
    }
}
