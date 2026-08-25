using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Resumes
{
    public class ResumeGeneratorService(IResumeRepository repository, IAiBulletChooser aibulletChooser) : IResumeGeneratorService
    {
        public async Task<ResumeResponse> GenerateResumeReviewAsync(int resumeId, string jobDescription)
        {
            var resume = await repository.GetResumeByIdAsync(resumeId)
                ?? throw new NotImplementedException($"Resume with id {resumeId} was not found during resume generation");


            var bulletSelectionContext = GetBulletSelectionContext(resume);

            var actualBullets = await aibulletChooser.ChooseBullets(bulletSelectionContext, jobDescription);

            return CreateCompanyGeneratedResume(resume, actualBullets);
        }

        private ResumeResponse CreateCompanyGeneratedResume(Resume resume, Dictionary<string, IReadOnlyList<string>> actualBullets)
        {
            //var companyGeneratedResume = new CompanyGeneratedResume
            //{
            //    ResumeId = resume.Id,
            //    Companies = new List<CompanyGeneratedResume.CompanyGenerated>()
            //};
            //foreach (var company in resume.Companies)
            //{
            //    if (actualBullets.TryGetValue(company.Name, out var bullets))
            //    {
            //        var companyGenerated = new CompanyGeneratedResume.CompanyGenerated
            //        {
            //            Name = company.Name,
            //            Bullets = bullets
            //        };
            //        companyGeneratedResume.Companies.Add(companyGenerated);
            //    }
            //}
            throw new NotImplementedException();
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
