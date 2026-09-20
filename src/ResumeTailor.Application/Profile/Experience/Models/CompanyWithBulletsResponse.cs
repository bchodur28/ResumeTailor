

namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record CompanyWithBulletsResponse(CompanyResponse Company, IReadOnlyCollection<BulletReponse> Bullets);
