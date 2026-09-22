using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

public interface IResumeManagementService
{
    Task<int> SaveGeneratedResumeAsync(ResumeRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ResumeListItemResponse>> GetGeneratedResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<ResumeDetailsResponse> GetResumeDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateGeneratedResumeAsync(int id, GeneratedResumeRequest request, CancellationToken cancellationToken = default);
    Task DeleteGeneratedResumeAsync(int id, CancellationToken cancellationToken = default);

    Task CreateCompanySelectionsAsync(int generatedResumeId, IEnumerable<ResumeCompanySelectionRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateCompanySelectionAsync(int id, ResumeCompanySelectionRequest request, CancellationToken cancellationToken = default);
    Task DeleteCompanySelectionAsync(int id, CancellationToken cancellationToken = default);

    Task CreateEducationSelectionsAsync(int generatedResumeId, IEnumerable<ResumeEducationSelectionRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateEducationSelectionAsync(int id, ResumeEducationSelectionRequest request, CancellationToken cancellationToken = default);
    Task DeleteEducationSelectionAsync(int id, CancellationToken cancellationToken = default);

    Task CreateProjectSelectionsAsync(int generatedResumeId, IEnumerable<ResumeProjectSelectionRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateProjectSelectionAsync(int id, ResumeProjectSelectionRequest request, CancellationToken cancellationToken = default);
    Task DeleteProjectSelectionAsync(int id, CancellationToken cancellationToken = default);

    Task CreateResumeBulletsAsycn(int resumeCompanyId, IEnumerable<ResumeBulletRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateResumeBulletAsync(int id, ResumeBulletRequest request, CancellationToken cancellationToken = default);
    Task DeleteResumeBulletAsync(int id, CancellationToken cancellationToken = default);

}
