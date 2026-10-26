using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Application.Common.Models
{
    public sealed record AiContext(string prompt, string jsonSchemaFormatName, string jsonSchema);
}
