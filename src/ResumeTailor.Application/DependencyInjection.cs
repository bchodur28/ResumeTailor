using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Extraction;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Profile.Accounts;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Education;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Profile.Experience;
using ResumeTailor.Application.Profile.Experience.Interfaces;

namespace ResumeTailor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISiteExtractionDefinitionService, SiteExtractionDefinitionService>();
        services.AddScoped<IFieldExtractionDefinitionService, FieldExtractionDefinitionService>();
        services.AddScoped<IFieldPatternService, FieldPatternService>();
        services.AddScoped<IResumeManagementService, ResumeManagementService>();
        services.AddScoped<IResumeGeneratorService, ResumeGeneratorService>();
        services.AddScoped<IResumeDataProvider, ResumeDataProvider>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IExperenceService, ExperienceService>();

        return services;
    }
}
