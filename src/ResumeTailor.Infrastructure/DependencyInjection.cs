using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Infrastructure.Persistence;


namespace ResumeTailor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databasePath = configuration["Database:Path"];

        if (string.IsNullOrWhiteSpace(databasePath))
        {
            var applicationDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ResumeTailor");
            Directory.CreateDirectory(applicationDataDirectory);
            databasePath = Path.Combine(applicationDataDirectory, "resume-tailor.db");
        }
        
        services.AddDbContext<ResumeTailorDbContext>(options =>
        {
            options.UseSqlite($"Data Source={databasePath}");
        });

        return services;
    }
}
