using ResumeTailor.Domain.Accounts;
using ResumeTailor.Domain.GeneratedResumes;

namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record ResumeSourceData(
    Account Account,
    GeneratedResume GeneratedResume,
    IReadOnlyCollection<Company> Companies,
    IReadOnlyCollection<Education> Educations,
    IReadOnlyCollection<Project> Projects
    );
