using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Resumes.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetResumeByIdAsync(int id, CancellationToken cancellationToken = default);
        Task CreateResume(Resume resume, CancellationToken cancellationToken = default);
        Task<bool> ResumeExistsAsync(int id, CancellationToken cancellationToken = default);
        Task CreateCompanyAsync(Company company, CancellationToken cancellationToken = default);
        Task<bool> CompanyExistsAsync(int id, CancellationToken cancellationToken = default);
        Task CreateBulletAsync(Bullet bullet, CancellationToken cancellationToken = default);
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
