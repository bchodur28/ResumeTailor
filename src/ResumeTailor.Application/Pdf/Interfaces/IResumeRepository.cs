using ResumeTailor.Domain.Resume;

namespace ResumeTailor.Application.Pdf.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetResumeById(int id, CancellationToken cancellationToken = default);
        Task CreateCompany(Company company, CancellationToken cancellationToken = default);
        Task UpdateCompany(Company company, CancellationToken cancellationToken = default);
        Task CreateBullet(Bullet bullet, CancellationToken cancellationToken = default);
        Task UpdateBullet(Bullet bullet, CancellationToken cancellationToken = default);
    }
}
