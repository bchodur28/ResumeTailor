using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Extraction;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.JobApplications;
using ResumeTailor.Application.JobApplications.Interfaces;

namespace ResumeTailor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISiteExtractionDefinitionService, SiteExtractionDefinitionService>();
        services.AddScoped<IFieldExtractionDefinitionService, FieldExtractionDefinitionService>();
        services.AddScoped<IFieldPatternService, FieldPatternService>();
        services.AddScoped<IGeneratedResumeManagementService, GeneratedResumeManagementService>();
        services.AddScoped<IResumeGeneratorService, ResumeGeneratorService>();
        services.AddScoped<IGeneratedResumeDataProvider, GeneratedResumeDataProvider>();
        services.AddScoped<IJobApplicationService, JobApplicationService>();

        return services;
    }
}
