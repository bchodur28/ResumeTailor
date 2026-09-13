using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Extraction;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

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

        return services;
    }
}
