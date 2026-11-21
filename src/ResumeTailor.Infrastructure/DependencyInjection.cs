using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Responses;
using QuestPDF.Infrastructure;
using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Rendering.Interfaces;
using ResumeTailor.Application.Library.Interfaces;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Infrastructure.AI;
using ResumeTailor.Infrastructure.Pdf;
using ResumeTailor.Infrastructure.Persistence;
using ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;
using ResumeTailor.Infrastructure.Persistence.Repositories.Extraction;
using ResumeTailor.Infrastructure.Persistence.Repositories.GeneratedResumes;
using ResumeTailor.Infrastructure.Persistence.Repositories.Profile;
using ResumeTailor.Infrastructure.Persistence.Repositories.Resumes;
using ResumeTailor.Infrastructure.Services;


namespace ResumeTailor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        QuestPDF.Settings.License = LicenseType.Evaluation;
        QuestPDF.Settings.UseSystemFonts = true;

        var databasePath = configuration["Database:Path"];

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Database connection string is missing.");

        services.AddDbContextFactory<ResumeTailorDbContext>(
            options => options.UseSqlServer(connectionString),
            ServiceLifetime.Scoped);

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
        services.AddScoped<IResumeRepository, ResumeRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IEducationRepository, EducationRepository>();
        services.AddScoped<IResumePdfGenerator, QuestPdfResumeGenerator>();
        services.AddScoped<ILibraryExperienceRepository, LibraryExperienceRepository>();
        services.AddScoped<IAiGenerator<AiResumeDataOutput>, OpenAiGenerator<AiResumeDataOutput>>();
        services.AddScoped<ICreditWalletRepository, CreditWalletRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICreditReservationRepository, CreditReservationRepository>();
        services.AddScoped<ICreditReservationService, CreditReservationService>();
        services.AddScoped<IResumeFinalizationService, ResumeFinalizationService>();
        services.AddScoped<ICreditReservationRecoveryService, CreditReservationRecoveryService>();
        services.AddScoped<IResumeGenerationOperationService, ResumeGenerationOperationService>();
        services.AddScoped<IResumeGenerationOperationRepository, ResumeGenerationOperationRepository>();

        return services;
    }
}
