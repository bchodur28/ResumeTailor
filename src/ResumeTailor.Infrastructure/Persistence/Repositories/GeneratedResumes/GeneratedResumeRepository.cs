using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.GeneratedResumes;

public class GeneratedResumeRepository(ResumeTailorDbContext dbContext) : IGeneratedResumeRepository
{
    // Resumes
    public async Task<IReadOnlyCollection<GeneratedResume>> GetGeneratedResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.GeneratedResumes
            .Where(gr => gr.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<GeneratedResume?> GetGeneratedResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.GeneratedResumes
            .Include(x => x.CompanySelections)
                .ThenInclude(x => x.Bullets)
            .Include(x => x.EducationSelections)
            .Include(x => x.ProjectSelections)
            .Include(x => x.AiAnalysis!)
                .ThenInclude(x => x.Insights)
            .Include(x => x.AiMetaData)
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<GeneratedResume?> GetGeneratedResumeForUpdatingAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.GeneratedResumes
            .FirstOrDefaultAsync(gr => gr.Id == id, cancellationToken);
    }

    public async Task CreateGeneratedResumeAsync(GeneratedResume generatedResume, CancellationToken cancellationToken = default)
    {
        await dbContext.GeneratedResumes.AddAsync(generatedResume, cancellationToken);
    }

    public void DeleteGeneratedResumeAsync(GeneratedResume generatedResume)
    {
        dbContext.GeneratedResumes.Remove(generatedResume);
    }

    public async Task<bool> GeneratedResumeExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.GeneratedResumes.AnyAsync(gr => gr.Id == id, cancellationToken);
    }

    // Company Selection
    public async Task<ResumeCompanySelection?> GetCompanySelectionForUpdating(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeCompanySelections
            .SingleOrDefaultAsync(rcs => rcs.Id == id, cancellationToken);
    }
    public async Task CreateCompanySelectionAsync(ResumeCompanySelection companySelection, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeCompanySelections.AddAsync(companySelection, cancellationToken).AsTask();
    }

    public async Task CreateCompanySelectionsAsync(IEnumerable<ResumeCompanySelection> companySelections, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeCompanySelections.AddRangeAsync(companySelections, cancellationToken);
    }

    public void DeleteCompanySelection(ResumeCompanySelection companySelection)
    {
        dbContext.ResumeCompanySelections.Remove(companySelection);
    }

    public async Task<bool> CompanySelectionExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeCompanySelections.AnyAsync(rcs => rcs.Id == id, cancellationToken);
    }


    // Education Selection
    public async Task<ResumeEducationSelection?> GetEducationSelectionForUpdating(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeEducationSelections
            .SingleOrDefaultAsync(resumeEducationSelection => resumeEducationSelection.Id == id, cancellationToken);
    }

    public async Task CreateEducationSelectionAsync(ResumeEducationSelection educationSelection, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeEducationSelections.AddAsync(educationSelection, cancellationToken).AsTask();
    }

    public async Task CreateEducationSelectionsAsync(IEnumerable<ResumeEducationSelection> educationSelections, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeEducationSelections.AddRangeAsync(educationSelections, cancellationToken);
    }

    public void DeleteEducationSelection(ResumeEducationSelection educationSelection)
    {
        dbContext.ResumeEducationSelections.Remove(educationSelection);
    }


    // Project Selection
    public async Task<ResumeProjectSelection?> GetProjectSelectionForUpdating(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeProjectSelections
            .SingleOrDefaultAsync(resumeEducationSelection => resumeEducationSelection.Id == id, cancellationToken);
    }

    public async Task CreateProjectSelectionAsync(ResumeProjectSelection projectSelection, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeProjectSelections.AddAsync(projectSelection, cancellationToken).AsTask();
    }

    public async Task CreateProjectSelectionsAsync(IEnumerable<ResumeProjectSelection> projectSelection, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeProjectSelections.AddRangeAsync(projectSelection, cancellationToken);
    }

    public void DeleteProjectSelection(ResumeProjectSelection projectSelection)
    {
        dbContext.ResumeProjectSelections.Remove(projectSelection);
    }


    // Resume Bullets
    public async Task<ResumeBullet?> GetResumeBulletForUpdating(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeBullets
            .SingleOrDefaultAsync(resumeEducationSelection => resumeEducationSelection.Id == id, cancellationToken);
    }

    public async Task CreateResumeBulletAsync(ResumeBullet bullet, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeBullets.AddAsync(bullet, cancellationToken).AsTask();
    }

    public async Task CreateResumeBulletsAysnc(IEnumerable<ResumeBullet> bullets, CancellationToken cancellationToken = default)
    {
        await dbContext.ResumeBullets.AddRangeAsync(bullets, cancellationToken);
    }

    public void DeleteResumeBullet(ResumeBullet bullet)
    {
        dbContext.ResumeBullets.Remove(bullet);
    }


    // Save Changes
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    
}
