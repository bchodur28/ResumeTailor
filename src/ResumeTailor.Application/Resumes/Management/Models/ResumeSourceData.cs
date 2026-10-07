using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Resumes.Management.Models;

public sealed record ResumeSourceData(
    Account Account,
    IReadOnlyCollection<Company> Companies,
    IReadOnlyCollection<Education> Education,
    IReadOnlyCollection<Project> Projects);
