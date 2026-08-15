using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Resumes;

public class Resume : Entity
{
    public string PersonName { get; private set; } = string.Empty;
    public string Profession { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;

    private readonly List<Company> _companies = [];
    public IReadOnlyCollection<Company> Companies => _companies.AsReadOnly();

    public string? College { get; private set; }
    public string? Degree { get; private set; }
    public string? Major { get; private set; }
    public string? CollegeStatus { get; private set; }
    public string? PersonalSite1 { get; private set; }
    public string? PersonalSite2 { get; private set; }
    public string? PersonalSite3 { get; private set; }

    public Resume(string personName, string profession, string email, string phoneNumber, string? college = null, string? degree = null, string? major = null, string? collegeStatus = null, string? personalSite1 = null, string? personalSite2 = null, string? personalSite3 = null)
    { 
        PersonName = personName;
        Profession = profession;
        Email = email;
        PhoneNumber = phoneNumber;
        College = college;
        Degree = degree;
        Major = major;
        CollegeStatus = collegeStatus;
        PersonalSite1 = personalSite1;
        PersonalSite2 = personalSite2;
        PersonalSite3 = personalSite3;
    }

    public void Update(string personName, string profession, string email, string phoneNumber, string? college = null, string? degree = null, string? major = null, string? collegeStatus = null, string? personalSite1 = null, string? personalSite2 = null, string? personalSite3 = null)
    {
        PersonName = personName;
        Profession = profession;
        Email = email;
        PhoneNumber = phoneNumber;
        College = college;
        Degree = degree;
        Major = major;
        CollegeStatus = collegeStatus;
        PersonalSite1 = personalSite1;
        PersonalSite2 = personalSite2;
        PersonalSite3 = personalSite3;
        MarkUpdated();
    }
}
