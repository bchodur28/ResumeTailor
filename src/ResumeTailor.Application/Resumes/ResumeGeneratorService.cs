using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Resumes
{
    public class ResumeGeneratorService(IResumeRepository repository, IAiBulletChooser aibulletChooser) : IResumeGeneratorService
    {
        public async Task<ResumeGenerationResult> GenerateResumeReviewAsync(int resumeId, string jobDescription, CancellationToken cancellationToken = default)
        {
            var resume = await repository.GetResumeByIdAsync(resumeId, cancellationToken)
                ?? throw new KeyNotFoundException($"Resume with id {resumeId} was not found during resume generation");


            var bulletSelectionContext = GetBulletSelectionContext(resume);

            var bulletSelectionResult = await aibulletChooser.ChooseBullets(bulletSelectionContext, jobDescription, cancellationToken);

            var companyGeneratedResume = CreateCompanyGeneratedResume(resume, bulletSelectionResult.CompanyBullets);

            return new ResumeGenerationResult(companyGeneratedResume, bulletSelectionResult.Usage);
        }

        private CompanyGeneratedResume CreateCompanyGeneratedResume(Resume resume, Dictionary<string, IReadOnlyList<string>> companyBullets)
        {
            var companyGeneratedResume = new CompanyGeneratedResume(
                resume.PersonName,
                resume.Profession,
                resume.Email,
                resume.PhoneNumber,
                resume.College,
                resume.Degree,
                resume.Major,
                resume.CollegeStatus,
                resume.PersonalSite1,
                resume.PersonalSite2,
                resume.PersonalSite3,
                Skills: new List<string>(),
                Companies: new List<GeneratedCompany>());

            foreach (var company in resume.Companies)
            {
                var generatedCompany = new GeneratedCompany(
                    company.Name,
                    company.Position,
                    company.WorkingStatus,
                    company.Location,
                    Bullets: GetBulletsFromCampanyBulletsDictionary(companyBullets, company.Name)
                    );

                companyGeneratedResume.Companies.Add(generatedCompany);
            }

            return companyGeneratedResume;
        }

        private static List<string> GetBulletsFromCampanyBulletsDictionary(Dictionary<string, IReadOnlyList<string>> companyBullets, string company)
        {
            return companyBullets.TryGetValue(company, out var bullets)
                ? bullets.ToList()
                : new List<string>();
        }

        private List<BulletSelectionContext> GetBulletSelectionContext(Resume resume)
        {
            List<BulletSelectionContext> bullets = new();
            foreach (var company in resume.Companies)
            {
                if (company.Bullets.Count > 0 && company.GenerateBullets)
                {
                    var bulletMetaData = new BulletSelectionContext(
                        Bullets: company.Bullets.Select(b => b.Value).ToList(),
                        Company: company.Name,
                        MaxBullets: company.MaxGeneratedBulletCount,
                        AdditionalInstruction: ""
                        );

                    bullets.Add( bulletMetaData );
                }
            }
            return bullets;
        }
    }
}
