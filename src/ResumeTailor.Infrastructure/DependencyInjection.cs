using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Responses;
using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.JobApplications.Interfaces;
using ResumeTailor.Infrastructure.AI;
using ResumeTailor.Infrastructure.Persistence;
using ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;
using ResumeTailor.Infrastructure.Persistence.Repositories.Extraction;
using ResumeTailor.Infrastructure.Persistence.Repositories.GeneratedResumes;
using ResumeTailor.Infrastructure.Persistence.Repositories.JobApplications;


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
            services.AddScoped<IResumeAiGenerator, OpenAIResumeGenerator>();
        } else
        {
            services.AddScoped<IResumeAiGenerator, OpenAIResumeGenerator>();
        }

        services.AddScoped<ISiteExtractionDefinitionRepository, SiteExtractionDefinitionRepository>();
        services.AddScoped<IFieldExtractionDefinitionRepository, FieldExtractionDefinitionRepository>();
        services.AddScoped<IFieldPatternRepository, FieldPatternRepository>();
        services.AddScoped<IGeneratedResumeRepository, GeneratedResumeRepository>();
        services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
        services.AddScoped<IExperienceRepository, ExperienceRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }
}
