using ResumeTailor.Application.Contracts.Accounts;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Application.Resumes.Management;

public sealed class ResumeDetailsAssembler
{
    public static ResumeDetailsResponse Assemble(ResumeSourceData data, Resume resume)
    {
        var selectedCompanies = data.Companies
            .Select(company =>
            {
                var selection = resume.CompanySelections
                    .Single(x => x.CompanyId == company.Id);

                return new ResumeCompanyResponse(
                    CompanyId: company.Id,
                    SelectionId: selection.Id,
                    Name: company.Name,
                    Title: company.Title,
                    Location: company.Location,
                    Started: company.Started,
                    Ended: company.Ended,
                    Bullets: selection.Bullets
                        .OrderBy(b => b.SortOrder)
                        .Select(b =>
                        {
                            var bulletsById = company.Bullets.ToDictionary(x => x.Id);

                            var sourceBullet =
                                b.SourceBulletId is int sourceBulletId &&
                                bulletsById.TryGetValue(sourceBulletId, out var bullet)
                                    ? bullet
                                    : null;

                            return new ResumeBulletResponse(
                                Id: b.Id,
                                SourceBulletId: b.SourceBulletId,
                                Value: b.Value,
                                AlternativeValue: b.AlternativeValue,
                                SortOrder: b.SortOrder,
                                IsSourceDeleted: sourceBullet?.IsDeleted == true);
                        })
                        .ToList());
            })
            .ToList();

        var selectedEducations = data.Education
            .Select(education =>
            {
                var selection = resume.EducationSelections
                    .Single(x => x.EducationId == education.Id);

                return new EducationResponse(
                    Id: education.Id,
                    SelectionId: selection.Id,
                    SchoolName: education.SchoolName,
                    Degree: education.Degree,
                    Major: education.Major,
                    Started: education.Started,
                    Ended: education.Ended,
                    UseForResume: education.UseForResume);
            })
            .ToList();

        var selectedProjects = data.Projects
            .Select(project =>
            {
                var selection = resume.ProjectSelections
                    .Single(x => x.ProjectId == project.Id);

                return new ProjectResponse(
                    Id: project.Id,
                    SelectionId: selection.Id,
                    Name: project.Name,
                    Description: project.Description,
                    Started: project.Started,
                    Ended: project.Ended,
                    TechStack: project.TechStack,
                    Link: project.Link,
                    UseForResume: project.UseForResume);
            })
            .ToList();

        var appearance = new ResumeAppearanceResponse(
            Id: resume.Appearance?.Id ?? -1,
            TitleFontSize: resume.Appearance?.TitleFontSize ?? 18,
            SectionHeaderFontSize: resume.Appearance?.SectionHeaderFontSize ?? 14,
            MainBodyFontSize: resume.Appearance?.MainBodyFontSize ?? 10,
            FontFamily: resume.Appearance?.FontFamily ?? "calibri, sans-serif",
            FontColor: resume.Appearance?.FontColor ?? "#1a2e5b",
            TopHeaderAlignment: resume.Appearance?.TopHeaderAlignment ?? AlignmentType.Center);

        var resumeDetails = new ResumeDetailsResponse(
            Resume: new ResumeResponse(
                Id: resume.Id,
                AccountId: data.Account.Id,
                ResumeName: resume.Name,
                AiScore: resume.AiScore,
                AiScoreStaleness: resume.AiScoreStaleness,
                PersonName: data.Account.DisplayName,
                Profession: data.Account.Titles
                    .FirstOrDefault(t => t.IsPrimary)?.Value ?? string.Empty,
                Email: data.Account.Email,
                PhoneNumber: data.Account.PhoneNumber,
                Location: $"{data.Account.City}, {data.Account.State}",
                PersonalLinks: data.Account.PersonalLinks
                    .Select(link => new PersonalLinkResponse(
                        link.Id,
                        link.DisplayName,
                        link.Url))
                    .ToList(),
                Skills: data.Account.Skills
                    .Select(skill => new SkillResponse(
                        skill.Id,
                        skill.Value))
                    .ToList(),
                Companies: selectedCompanies,
                Education: selectedEducations,
                Projects: selectedProjects,
                Appearance: appearance),

            AiAnalysis: new AiAnalysisResult(
                resume.AiAnalysis?.Summary ?? string.Empty,
                resume.AiAnalysis?.Strengths
                    .Select(x => x.Value)
                    .ToList() ?? [],
                resume.AiAnalysis?.Weaknesses
                    .Select(x => x.Value)
                    .ToList() ?? []),

            JobPosting: new JobPostingResult(
                CompanyName: resume.JobPosting?.CompanyName,
                JobTitle: resume.JobPosting?.JobTitle,
                Location: resume.JobPosting?.Location,
                WorkStyle: resume.JobPosting?.WorkStyle,
                SalaryMin: resume.JobPosting?.SalaryMin,
                SalaryMax: resume.JobPosting?.SalaryMax,
                Salary: resume.JobPosting?.Salary,
                SalaryPeriod: resume.JobPosting?.SalaryPeriod,
                SalaryCurrency: resume.JobPosting?.SalaryCurrency),

            ApplicationTracking: new ApplicationTrackingResponse(
                Status: resume.ApplicationTracking?.Status ?? ApplicationStatus.Interested,
                Applied: resume.ApplicationTracking?.Applied,
                Interviewed: resume.ApplicationTracking?.Interviewed,
                OfferReceived: resume.ApplicationTracking?.OfferReceived,
                OfferAccepted: resume.ApplicationTracking?.OfferAccepted,
                Rejected: resume.ApplicationTracking?.Rejected),

            AiMetaData: new AiMetaDataResult(
                resume.AiMetaData?.Model ?? string.Empty,
                resume.AiMetaData?.InputTokens ?? 0,
                resume.AiMetaData?.OutputTokens ?? 0,
                resume.AiMetaData?.TotalTokens ?? 0,
                resume.AiMetaData?.Cost ?? 0));

        return resumeDetails;
    }
}
