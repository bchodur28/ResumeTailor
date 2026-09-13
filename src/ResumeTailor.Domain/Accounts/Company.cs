using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Accounts;

public sealed class Company : Entity
{
    public int AccountId { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateOnly Started { get; private set; }
    public DateOnly? Ended { get; private set; }
    public bool GenerateBullets { get; private set; }
    public int MaxGeneratedBulletCount { get; private set; }

    private readonly List<Bullet> _bullets = [];
    public IReadOnlyCollection<Bullet> Bullets => _bullets.AsReadOnly();

    public Company(int accountId, string name, string location, string title, DateOnly started, DateOnly? ended, bool generateBullets, int maxGeneratedBulletCount)
    {
        AccountId = accountId;
        Name = name;
        Location = location;
        Title = title;
        Started = started;
        Ended = ended;
        GenerateBullets = generateBullets;
        MaxGeneratedBulletCount = maxGeneratedBulletCount;
    }

    public void Update(string name, string location, string title, DateOnly started, DateOnly? ended, bool generateBullets, int maxGeneratedBulletCount)
    {
        Name = name;
        Location = location;
        Title = title;
        Started = started;
        Ended = ended;
        GenerateBullets = generateBullets;
        MaxGeneratedBulletCount = maxGeneratedBulletCount;
        MarkUpdated();
    }
}
