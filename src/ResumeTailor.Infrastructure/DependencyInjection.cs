using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Responses;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Infrastructure.AI;
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

        var openAiApiKey = configuration["OpenAI:ApiKey"];

        services.Configure<OpenAIOptions>(configuration.GetSection(OpenAIOptions.SectionName));

        if (!string.IsNullOrWhiteSpace(openAiApiKey))
        {
#pragma warning disable OPENAI001
            services.AddSingleton(new ResponsesClient(openAiApiKey));
#pragma warning restore OPENAI001
            services.AddScoped<IAiBulletChooser, OpenAIBulletChooser>();
        } else
        {
            services.AddScoped<IAiBulletChooser, OpenAIBulletChooser>();
        }

        services.AddScoped<ISiteExtractionDefinitionRepository, SiteExtractionDefinitionRepository>();
        services.AddScoped<IFieldExtractionDefinitionRepository, FieldExtractionDefinitionRepository>();
        services.AddScoped<IFieldPatternRepository, FieldPatternRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();

        return services;
    }
}
