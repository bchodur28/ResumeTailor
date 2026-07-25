using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Domain.Common;

public abstract class Entity
{
    public int Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; protected set; }

    protected void MarkUpdated()
    {
        UpdatedDate = DateTime.Now;
    }
}
