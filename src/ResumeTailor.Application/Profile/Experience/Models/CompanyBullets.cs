using ResumeTailor.Domain.Profile;


namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record CompanyBullets(int CompanyId, string CompanyName, IReadOnlyCollection<Bullet> Bullets);
