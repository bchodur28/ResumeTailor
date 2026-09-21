namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record CompanyBulletsResponse(int CompanyId, string CompanyName, IReadOnlyCollection<BulletReponse> Bullets);
