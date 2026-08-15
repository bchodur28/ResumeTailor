using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Resumes;

public class Company : Entity
{
    public int ResumeId { get; private set; }
    public Resume Resume { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;
    public string Position { get; private set; } = string.Empty;
    public string WorkingStatus { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;

    public bool GenerateBullets { get; private set; }
    public int MaxGeneratedBulletCount { get; private set; }

    public readonly List<Bullet> _bullets = [];
    public IReadOnlyCollection<Bullet> Bullets => _bullets.AsReadOnly();

    public Company(int resumeId, string name, string position, string workingStatus, string location, bool generateBullets, int maxGeneratedBulletCount)
    {
        ResumeId = resumeId;
        Name = name;
        Position = position;
        WorkingStatus = workingStatus;
        Location = location;
        GenerateBullets = generateBullets;
        MaxGeneratedBulletCount = maxGeneratedBulletCount;
    }

}
