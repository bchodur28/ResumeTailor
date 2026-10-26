using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Profile;

public sealed class LibraryEducation : Entity
{
    public int AccountId { get; private set; }

    public string InstitutionName { get; private set; } = string.Empty;
    public string Qualification { get; private set; } = string.Empty;
    public string? FieldOfStudy { get; private set; }
    public decimal? Gpa { get; private set; }

    public string? Location { get; private set; }

    public DateOnly Started { get; private set; }
    public DateOnly? Ended { get; private set; }

    public bool UseForResume { get; private set; }

    public LibraryEducation(int accountId, string institutionName, string qualification, string fieldOfStudy, decimal? gpa, string? location, DateOnly started, DateOnly? ended, bool useForResume)
    {
        AccountId = accountId;
        InstitutionName = institutionName;
        Qualification = qualification;
        FieldOfStudy = fieldOfStudy;
        Gpa = gpa;
        Location = location;
        Started = started;
        Ended = ended;
        UseForResume = useForResume;
    }

    public void Update(string institutionName, string qualification, string? fieldOfStudy, decimal? gpa, string? location, DateOnly started, DateOnly? ended, bool useForResume)
    {
        InstitutionName = institutionName;
        Qualification = qualification;
        FieldOfStudy = fieldOfStudy;
        Gpa = gpa;
        Location = location;
        Started = started;
        Ended = ended;
        UseForResume = useForResume;
        MarkUpdated();
    }
}
