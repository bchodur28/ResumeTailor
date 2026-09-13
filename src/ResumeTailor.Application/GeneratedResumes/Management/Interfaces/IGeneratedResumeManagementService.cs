using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Management.Models;
using ResumeTailor.Domain.GeneratedResumes;

namespace ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

public interface IGeneratedResumeManagementService
{
    Task<int> SaveGeneratedResumeAsync(SaveGeneratedResumeRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<GeneratedResumeListItemResponse>> GetGeneratedResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<GeneratedResumeDetailsResponse> GetGeneratedResumeAsync(int id, CancellationToken cancellationToken = default);
    Task CreateGeneratedResumeAsync(GeneratedResumeRequest request, CancellationToken cancellationToken = default);
    Task UpdateGeneratedResumeAsync(int id, GeneratedResumeRequest request, CancellationToken cancellationToken = default);
    Task DeleteGeneratedResumeAsync(int id, CancellationToken cancellationToken = default);

    Task CreateCompanySelectionAsync(int generatedResumeId, ResumeCompanySelectionRequest request, CancellationToken cancellationToken = default);
    Task CreateCompanySelectionsAsync(int generatedResumeId, IEnumerable<ResumeCompanySelectionRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateCompanySelectionAsync(int id, ResumeCompanySelectionRequest request, CancellationToken cancellationToken = default);
    Task DeleteCompanySelectionAsync(int id, CancellationToken cancellationToken = default);

    Task CreateEducationSelectionAsync(int generatedResumeId, ResumeEducationSelectionRequest request, CancellationToken cancellationToken = default);
    Task CreateEducationSelectionsAsync(int generatedResumeId, IEnumerable<ResumeEducationSelectionRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateEducationSelectionAsync(int id, ResumeEducationSelectionRequest request, CancellationToken cancellationToken = default);
    Task DeleteEducationSelectionAsync(int id, CancellationToken cancellationToken = default);

    Task CreateProjectSelectionAsync(int generatedResumeId, ResumeProjectSelectionRequest request, CancellationToken cancellationToken = default);
    Task CreateProjectSelectionsAsync(int generatedResumeId, IEnumerable<ResumeProjectSelectionRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateProjectSelectionAsync(int id, ResumeProjectSelectionRequest request, CancellationToken cancellationToken = default);
    Task DeleteProjectSelectionAsync(int id, CancellationToken cancellationToken = default);

    Task CreateResumeBulletAsync(int resumeCompanyId, ResumeBulletRequest request, CancellationToken cancellationToken = default);
    Task CreateResumeBulletsAsycn(int resumeCompanyId, IEnumerable<ResumeBulletRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateResumeBulletAsync(int id, ResumeBulletRequest request, CancellationToken cancellationToken = default);
    Task DeleteResumeBulletAsync(int id, CancellationToken cancellationToken = default);

}
