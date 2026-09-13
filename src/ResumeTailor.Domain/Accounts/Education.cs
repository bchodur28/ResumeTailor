using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Accounts;

public sealed class Education : Entity
{
    public int AccountId { get; private set; }

    public string SchoolName { get; private set; } = string.Empty;
    public string Degree { get; private set; } = string.Empty;
    public string Major { get; private set; } = string.Empty;
    public DateOnly Started { get; private set; }
    public DateOnly? Ended { get; private set; }

    public bool UseForResume { get; private set; }

    public Education(int accountId, string schoolName, string degree, string major, DateOnly started, DateOnly? ended, bool useForResume)
    {
        AccountId = accountId;
        SchoolName = schoolName;
        Degree = degree;
        Major = major;
        Started = started;
        Ended = ended;
        UseForResume = useForResume;
    }

    public void Update(string schoolName, string degree, string major, DateOnly started, DateOnly? ended, bool useForResume)
    {
        SchoolName = schoolName;
        Degree = degree;
        Major = major;
        Started = started;
        Ended = ended;
        UseForResume = useForResume;
        MarkUpdated();
    }
}
