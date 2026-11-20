namespace ResumeTailor.Domain.Common;

public abstract class Entity : BaseEntity
{
    public DateTime? UpdatedDate { get; protected set; }

    protected void MarkUpdated()
    {
        UpdatedDate = DateTime.Now;
    }
}
