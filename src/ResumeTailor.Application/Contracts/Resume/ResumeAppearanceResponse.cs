using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeAppearanceResponse(
    int Id,
    int TitleFontSize,
    int SectionHeaderFontSize,
    int MainBodyFontSize,
    string FontFamily,
    string FontColor,
    AlignmentType TopHeaderAlignment);
