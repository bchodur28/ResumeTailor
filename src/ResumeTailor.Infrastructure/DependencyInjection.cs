using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Infrastructure.Persistence;
using ResumeTailor.Infrastructure.Persistence.Repositories;


namespace ResumeTailor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databasePath = configuration["Database:Path"];

        if (string.IsNullOrWhiteSpace(databasePath))
        {
            var applicationDataDirectory = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "ResumeTailor");

            databasePath = Path.Combine(applicationDataDirectory, "resume-tailor.db");
        }

        var databaseDirectory = Path.GetDirectoryName(databasePath);

        if (!string.IsNullOrWhiteSpace(databaseDirectory))
        {
            Directory.CreateDirectory(databaseDirectory);
        }
        
        services.AddDbContext<ResumeTailorDbContext>(options =>
        {
            options.UseSqlite($"Data Source={databasePath}");
        });

        services.AddScoped<ISiteExtractionDefinitionRepository, SiteExtractionDefinitionRepository>();
        services.AddScoped<IFieldExtractionDefinitionRepository, FieldExtractionDefinitionRepository>();
        services.AddScoped<IFieldSelectorRepository, FieldSelectorRepository>();

        return services;
    }
}
