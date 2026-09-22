using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

public interface IResumeRepository
{
    // Generated Resume
    Task<IReadOnlyCollection<GeneratedResume>> GetResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<GeneratedResume?> GetResumeAsync(int id, CancellationToken cancellationToken = default);
    Task<GeneratedResume?> GetResumeForUpdatingAsync(int id, CancellationToken cancellationToken = default);
    Task CreateResumeAsync(GeneratedResume generatedResume, CancellationToken cancellationToken = default);
    void DeleteResumeAsync(GeneratedResume generatedResume);
    Task<bool> ResumeExistsAsync(int id, CancellationToken cancellationToken = default);

    // Resume Company Selection
    Task<ResumeCompanySelection?> GetCompanySelectionForUpdating(int id, CancellationToken cancellationToken = default);
    Task CreateCompanySelectionAsync(ResumeCompanySelection companySelection, CancellationToken cancellationToken = default);
    Task CreateCompanySelectionsAsync(IEnumerable<ResumeCompanySelection> companySelections, CancellationToken cancellationToken = default);
    void DeleteCompanySelection(ResumeCompanySelection companySelection);
    Task<bool> CompanySelectionExistsAsync(int id, CancellationToken cancellationToken = default);

    // Resume Education Selection
    Task<ResumeEducationSelection?> GetEducationSelectionForUpdating(int id, CancellationToken cancellationToken = default);
    Task CreateEducationSelectionAsync(ResumeEducationSelection educationSelection, CancellationToken cancellationToken = default);
    Task CreateEducationSelectionsAsync(IEnumerable<ResumeEducationSelection> educationSelections, CancellationToken cancellationToken = default);
    void DeleteEducationSelection(ResumeEducationSelection educationSelection);

    // Resume Project Selection
    Task<ResumeProjectSelection?> GetProjectSelectionForUpdating(int id, CancellationToken cancellationToken = default);
    Task CreateProjectSelectionAsync(ResumeProjectSelection projectSelection, CancellationToken cancellationToken = default);
    Task CreateProjectSelectionsAsync(IEnumerable<ResumeProjectSelection> projectSelections, CancellationToken cancellationToken = default);
    void DeleteProjectSelection(ResumeProjectSelection projectSelection);

    Task<ResumeBullet?> GetResumeBulletForUpdating(int id, CancellationToken cancellationToken = default);
    Task CreateResumeBulletAsync(ResumeBullet bullet, CancellationToken cancellationToken = default);
    Task CreateResumeBulletsAysnc(IEnumerable<ResumeBullet> bullets, CancellationToken cancellationToken = default);
    void DeleteResumeBullet(ResumeBullet bullet);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
