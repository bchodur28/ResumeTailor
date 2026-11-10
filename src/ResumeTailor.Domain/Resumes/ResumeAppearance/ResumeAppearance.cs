using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Resumes.ResumeAppearance;

public sealed class ResumeAppearance : Entity
{
    public int ResumeId { get; private set; }
    public int TitleFontSize { get; private set; } = 18;
    public int SectionHeaderFontSize { get; private set; } = 14;
    public int MainBodyFontSize { get; private set; } = 10;
    public string FontFamily { get; private set; } = "calibri, sans-serif";
    public string FontColor { get; private set; } = "#1a2e5b";
    public AlignmentType TopHeaderAlignment { get; private set; } = AlignmentType.Center;
    public bool HideCertificationIssuer { get; private set; }

    public ResumeAppearance(int titleFontSize, int sectionHeaderFontSize, int mainBodyFontSize, string fontFamily, string fontColor, AlignmentType topHeaderAlignment, bool hideCertificationIssuer)
    {
        TitleFontSize = titleFontSize;
        SectionHeaderFontSize = sectionHeaderFontSize;
        MainBodyFontSize = mainBodyFontSize;
        FontFamily = fontFamily;
        FontColor = fontColor;
        TopHeaderAlignment = topHeaderAlignment;
        HideCertificationIssuer = hideCertificationIssuer;
    }

    public static ResumeAppearance CreateDefault()
    {
        return new ResumeAppearance(18, 14, 10, "calibri, sans-serif", "#1a2e5b", AlignmentType.Center, false);
    }

    public void Update(int titleFontSize, int sectionHeaderFontSize, int mainBodyFontSize, string fontFamily, string fontColor, AlignmentType topHeaderAlignment, bool hideCertificationIssuer)
    {
        TitleFontSize = titleFontSize;
        SectionHeaderFontSize = sectionHeaderFontSize;
        MainBodyFontSize = mainBodyFontSize;
        FontFamily = fontFamily;
        FontColor = fontColor;
        TopHeaderAlignment = topHeaderAlignment;
        HideCertificationIssuer = hideCertificationIssuer;
        MarkUpdated();
    }
}
