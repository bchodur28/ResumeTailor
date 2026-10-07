using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;


namespace ResumeTailor.Application.Resumes.Generation;

public class ResumeGeneratorService(
    IResumeAiGenerator aiGenerator,
    IExperienceRepository experienceRepository,
    IResumeCreator resumeCreator) : IResumeGenerator
{
    public async Task<int> GenerateResumeAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default)
    {
        var companyBulletAiContext = await experienceRepository.GetCompaniesForAiGenerationAsync(accountId, cancellationToken);
        var resumeAiContext = new ResumeAiContext(jobDescription, companyBulletAiContext);

        var resumeGenerationResult = await aiGenerator.GenerateAsync(resumeAiContext, cancellationToken);

        var resumeId = await resumeCreator.CreateResumeAsync(accountId, resumeGenerationResult, cancellationToken);

        return resumeId;
    }
}
