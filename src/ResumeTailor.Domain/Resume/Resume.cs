

using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Resume;

public class Resume : Entity
{
    public string PersonName { get; private set; } = string.Empty;
    public string Profession { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;

    public readonly List<Company> _company = [];
    public IReadOnlyCollection<Company> Company => _company.AsReadOnly();

    public readonly List<string> _skills = [];
    public IReadOnlyCollection<string> Skills => _skills.AsReadOnly();

    public string? College { get; private set; }
    public string? Degree { get; private set; }
    public string? Major { get; private set; }
    public string? CollegeStatus { get; private set; }
    public string? PersonalSite1 { get; private set; }
    public string? PerosnalSite2 { get; private set; }
    public string? PersonalSite3 { get; private set; }

    public Resume(string personName, string profession, string email, string number, string? college = null, string? degree = null, string? major = null, string? collegeStatus = null, string? personalSite1 = null, string? personalSite2 = null, string? personalSite3 = null)
    { 
        PersonName = personName;
        Profession = profession;
        Email = email;
        Number = number;
        College = college;
        Degree = degree;
        Major = major;
        CollegeStatus = collegeStatus;
        PersonalSite1 = personalSite1;
        PerosnalSite2 = personalSite2;
        PersonalSite3 = personalSite3;
    }

    public void Update(string personName, string profession, string email, string number, string? college = null, string? degree = null, string? major = null, string? collegeStatus = null, string? personalSite1 = null, string? personalSite2 = null, string? personalSite3 = null)
    {
        PersonName = personName;
        Profession = profession;
        Email = email;
        Number = number;
        College = college;
        Degree = degree;
        Major = major;
        CollegeStatus = collegeStatus;
        PersonalSite1 = personalSite1;
        PerosnalSite2 = personalSite2;
        PersonalSite3 = personalSite3;
        MarkUpdated();
    }
}
