using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; } = DateTime.Now;
    }
}
