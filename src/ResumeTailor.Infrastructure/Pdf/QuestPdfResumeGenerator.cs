using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Rendering.Interfaces;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Infrastructure.Pdf;

public sealed class QuestPdfResumeGenerator : IResumePdfGenerator
{
    private const float PageMargin = 36;

    private const float SectionTopSpacing = 8;
    private const float SectionContentSpacing = 4;

    private const float CompanySpacing = 8;
    private const float BulletSpacing = 4;

    private const float BulletIndent = 14;
    private const float ProjectIndent = 14;

    public byte[] Generate(ResumeResponse resume)
    {
        var appearance = resume.Appearance;

        var document = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(PageMargin);
                page.PageColor(Colors.White);

                page.DefaultTextStyle(style => style
                    .FontFamily(GetFontFamily(appearance.FontFamily))
                    .FontSize(appearance.MainBodyFontSize)
                    .FontColor(Color.FromHex("#000")));

                page.Content()
                    .Column(column =>
                    {
                        ComposeHeader(column, resume);

                        ComposeSkills(column, resume);

                        ComposeCompanies(column, resume);

                        if (resume.Education.Count > 0)
                            ComposeEducation(column, resume);

                        if (resume.Projects.Count > 0)
                            ComposeProjects(column, resume);
                    });
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(
    ColumnDescriptor column,
    ResumeResponse resume)
    {
        var appearance = resume.Appearance;

        column.Item()
            .Column(header =>
            {
                Align(header.Item(), appearance.TopHeaderAlignment)
                    .Text(resume.PersonName)
                    .FontSize(appearance.TitleFontSize)
                    .SemiBold();

                Align(header.Item(), appearance.TopHeaderAlignment)
                    .Text(resume.Profession)
                    .FontSize(14)
                    .FontColor(Color.FromHex(appearance.FontColor));

                var city = resume.Location
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault()?
                    .Trim();

                var state = resume.Location
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .FirstOrDefault()?
                    .Trim();

                var stateCode = StateCodeMapping.GetStateCode(state ?? "");

                Align(header.Item(), appearance.TopHeaderAlignment)
                    .Text($"{city}, {stateCode}");

                Align(header.Item(), appearance.TopHeaderAlignment)
                    .Text(text =>
                    {
                        var hasPreviousItem = false;

                        if (!string.IsNullOrWhiteSpace(resume.Email))
                        {
                            text.Span(resume.Email)
                                .Underline()
                                .FontColor(Color.FromHex("#467886")); ;
                            hasPreviousItem = true;
                        }

                        if (!string.IsNullOrWhiteSpace(resume.PhoneNumber))
                        {
                            if (hasPreviousItem)
                                text.Span("   ");

                            text.Span(resume.PhoneNumber);
                            hasPreviousItem = true;
                        }

                        foreach (var link in resume.PersonalLinks)
                        {
                            if (hasPreviousItem)
                                text.Span("   ");

                            text.Hyperlink(link.DisplayName, link.Url)
                                .Underline()
                                .FontColor(Color.FromHex("#467886"));

                            hasPreviousItem = true;
                        }
                    });
            });
    }

    private static void ComposeSkills(ColumnDescriptor column, ResumeResponse resume)
    {
        SectionHeader(column, "SKILLS", resume.Appearance);

        column.Item()
            .PaddingTop(SectionContentSpacing)
            .Text(string.Join(
                ", ",
                resume.Skills.Select(skill => skill.Value)));
    }

    private static void ComposeCompanies(ColumnDescriptor column, ResumeResponse resume)
    {
        SectionHeader(column, "EXPERIENCE", resume.Appearance);

        var index = 0;
        foreach (var company in resume.Companies)
        {
            column.Item()
                .PaddingTop(index == 0 ? SectionContentSpacing : CompanySpacing)
                .Column(companyColumn =>
                {
                    var date = company.Ended is DateOnly ended
                        ? $"{company.Started:MMM-yyyy} - {company.Ended:MMM-yyyy}"
                        : $"{company.Started:MMM-yyyy} - Present";

                    TwoColumn(companyColumn, company.Name, date, bold: true);
                    TwoColumn(companyColumn, company.Title, company.Location ?? "Remote");

                    if (company.Bullets.Count > 0)
                    {
                        companyColumn
                            .Item()
                            .PaddingTop(SectionContentSpacing)
                            .Column(bullets =>
                            {
                                bullets.Spacing(BulletSpacing);

                                foreach (var bullet in company.Bullets)
                                {
                                    bullets
                                        .Item()
                                        .Row(row =>
                                        {
                                            row.ConstantItem(BulletIndent)
                                                .Text("•");

                                            row.RelativeItem()
                                                .Text(bullet.Value);
                                        });
                                }
                            });
                    }
                });

            index++;
        }
    }

    private static void ComposeEducation(ColumnDescriptor column, ResumeResponse resume)
    {
        SectionHeader(column, "EDUCATION", resume.Appearance);

        var index = 0;
        foreach(var education in resume.Education)
        {
            column
                .Item()
                .PaddingTop(index == 0 ? SectionContentSpacing : CompanySpacing)
                .Column(educationColumn =>
                {
                    var date = education.Ended is DateOnly ended
                        ? ended.ToString("MMM-yyyy")
                        : $"{education.Started:MMM-yyyy-dd} - Present";

                    TwoColumn(educationColumn, education.SchoolName, date, true);

                    TwoColumn(educationColumn, education.Degree, education.Major);
                });
            index++;
        }
    }

    private static void ComposeProjects(ColumnDescriptor column, ResumeResponse resume)
    {
        SectionHeader(column, "PROJECTS", resume.Appearance);

        var index = 0;
        foreach (var project in resume.Projects)
        {
            column
                .Item()
                .PaddingTop(index == 0 ? SectionContentSpacing : CompanySpacing)
                .Column(projectColumn =>
                {
                    var date = project.Ended is DateOnly ended
                        ? ended.ToString("MMM-yyyy")
                        : $"{project.Started:MMM-yyyy} - Present";

                    TwoColumn(projectColumn, project.Name, date, bold: true);

                    if (!string.IsNullOrWhiteSpace(project.Description))
                    {
                        projectColumn
                            .Item()
                            .PaddingTop(SectionContentSpacing)
                            .PaddingLeft(ProjectIndent)
                            .Text(project.Description);
                    }

                    if (!string.IsNullOrWhiteSpace(project.TechStack))
                    {
                        projectColumn
                            .Item()
                            .PaddingTop(SectionContentSpacing)
                            .PaddingLeft(ProjectIndent)
                            .Text(text =>
                            {
                                text.Span("Tech Stack: ").Bold();
                                text.Span(project.TechStack);
                            });
                    }

                    if (!string.IsNullOrWhiteSpace(project.Link))
                    {
                        projectColumn
                            .Item()
                            .PaddingTop(SectionContentSpacing)
                            .PaddingLeft(ProjectIndent)
                            .Row(row =>
                            {
                                row.AutoItem()
                                    .Text("Link: ")
                                    .Bold();

                                row.RelativeItem()
                                    .Hyperlink(project.Link)
                                    .Text(project.Link);
                            });
                    }
                });

            index++;
        }
    }

    private static void SectionHeader(ColumnDescriptor column, string title, ResumeAppearanceResponse appearance)
    {
        column
            .Item()
            .PaddingTop(SectionTopSpacing)
            .BorderBottom(0.75f)
            .PaddingBottom(1)
            .Text(title)
            .FontSize(appearance.SectionHeaderFontSize)
            .FontColor(Color.FromHex(appearance.FontColor))
            .SemiBold();
    }

    private static void TwoColumn(ColumnDescriptor column,string? left,string? right,bool bold = false)
    {
        column
            .Item()
            .Row(row =>
            {
                var leftText = row
                    .RelativeItem(3)
                    .Text(left ?? string.Empty);

                var rightText = row
                    .RelativeItem(2)
                    .Text(right ?? string.Empty)
                    .AlignRight();

                if (bold)
                {
                    leftText.Bold();
                    rightText.Bold();
                }
            });
    }

    private static IContainer Align(IContainer container, AlignmentType alignment)
    {
        return alignment switch
        {
            AlignmentType.Left => container.AlignLeft(),
            AlignmentType.Center => container.AlignCenter(),
            AlignmentType.Right => container.AlignRight(),
            _ => container.AlignCenter()
        };
    }

    private static string GetFontFamily(string fontFamily)
    {
        // Your React value may be something like:
        //
        // "calibri, sans-serif"
        //
        // QuestPDF wants an actual registered font family,
        // not a CSS font stack.
        return fontFamily
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .First()
            .Trim()
            .Trim('\'', '"');
    }
}
