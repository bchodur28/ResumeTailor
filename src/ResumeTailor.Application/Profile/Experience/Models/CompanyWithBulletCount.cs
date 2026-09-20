using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record CompanyWithBulletCount(Company company, int BulletCount);
