using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Extraction;

namespace ResumeTailor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISiteExtractionDefinitionService, SiteExtractionDefinitionService>();

        return services;
    }
}
