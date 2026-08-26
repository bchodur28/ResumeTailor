using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Extraction;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Resumes;
using ResumeTailor.Application.Resumes.Interfaces;

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

        return services;
    }
}
