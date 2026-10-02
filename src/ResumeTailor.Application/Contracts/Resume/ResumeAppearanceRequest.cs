
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeAppearanceRequest(
    int TitleFontSize,
    int SectionHeaderFontSize,
    int MainBodyFontSize,
    string FontFamily,
    string FontColor,
    AlignmentType TopHeaderAlignment);
