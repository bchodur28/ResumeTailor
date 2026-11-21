using ResumeTailor.Domain.Resumes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Application.Resumes.Generation.Models;

public record GenerationStartResult(
    int OperationId,
    int ReservationId,
    ResumeGenerationStatus Status,
    Guid LeaseToken,
    int? ResumeId);
