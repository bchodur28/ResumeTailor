using ResumeTailor.Application.Contracts.Bullets;

namespace ResumeTailor.Application.Contracts.Companies;

public sealed record CompanyBulletsResponse(int CompanyId, string CompanyName, IReadOnlyCollection<BulletReponse> Bullets);
