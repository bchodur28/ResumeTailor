using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Extraction;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Profile.Accounts;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Education;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Profile.Experience;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Resumes.Generation;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Management;
using ResumeTailor.Application.Resumes.Management.Interfaces;

namespace ResumeTailor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISiteExtractionDefinitionService, SiteExtractionDefinitionService>();
        services.AddScoped<IFieldExtractionDefinitionService, FieldExtractionDefinitionService>();
        services.AddScoped<IFieldPatternService, FieldPatternService>();
        services.AddScoped<IResumeManagementService, ResumeManagementService>();
        services.AddScoped<IResumeGenerator, ResumeGeneratorService>();
        services.AddScoped<IResumeDataProvider, ResumeDataProvider>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperenceService, ExperienceService>();
        services.AddScoped<IResumeUpdater, ResumeUpdater>();
        services.AddScoped<IResumeCreator, ResumeCreator>();

        return services;
    }
}
