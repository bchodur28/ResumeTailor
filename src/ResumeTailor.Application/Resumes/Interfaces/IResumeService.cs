using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Application.Resumes.Interfaces;

public interface IResumeService
{
    Task<ResumeResponse> GetResumeByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateResumeAsync(ResumeRequest request, CancellationToken cancellationToken = default);
    Task CreateCompanyAsync(CompanyRequest request, CancellationToken cancellationToken = default);
    Task CreateBulletAsync(BulletRequest request, CancellationToken cancellationToken = default);

}
