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
    private const float BulletTopSpacing = 8;
    private const float BulletSpacing = 4;

    private const float BulletIndent = 14;
    private const float ProjectIndent = 12;

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
                    .LineHeight(1.0f)
                    .FontColor(Color.FromHex("#000")));

                page.Content()
                    .Column(column =>
                    {
                        ComposeHeader(column, resume);
                        ComposeSummary(column, resume);
                        ComposeSkills(column, resume);
                        ComposeCompanies(column, resume);

                        if (resume.Education.Any(edu => edu.Type == EducationType.Degree))
                        {
                            ComposeEducation(column, resume);
                        }

                        if (resume.Education.Any(
                            edu => edu.Type is EducationType.Certificate or EducationType.Certification))
                        {
                            ComposeCertifcations(column, resume);
                        }

                        if (resume.Experiences.Any(
                            edu => edu.Type is ExperienceType.PersonalProject or ExperienceType.OpenSourceProject or ExperienceType.AcademicProject))
                        {
                            ComposeProjects(column, resume);
                        }

                    
                    });
            });
        })
            .WithMetadata(new DocumentMetadata {
                Title = $"{resume.PersonName} - Resume",
                Subject = "Professional Resume",
                Language = "en-US"
            })
            .WithSettings(new DocumentSettings {
                PDFUA_Conformance = PDFUA_Conformance.PDFUA_1
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
                    .SemanticHeading1()
                    .Text(resume.PersonName)
                    .FontSize(appearance.TitleFontSize)
                    .SemiBold();

                Align(header.Item(), appearance.TopHeaderAlignment)
                    .Text(resume.Profession)
                    .FontSize(14)
                    .FontColor(Color.FromHex(appearance.FontColor));
                    
                Align(header.Item(), appearance.TopHeaderAlignment)
                    .Text(resume.Location)
                    .FontSize(11);

                Align(header.Item(), appearance.TopHeaderAlignment)
                    .Text(text =>
                    {
                        var hasPreviousItem = false;

                        if (!string.IsNullOrWhiteSpace(resume.Email))
                        {
                            text.Span(resume.Email)
                                .Underline()
                                .FontColor(Color.FromHex("#467886"))
                                .FontSize(11);
                            hasPreviousItem = true;
                        }

                        if (!string.IsNullOrWhiteSpace(resume.PhoneNumber))
                        {
                            if (hasPreviousItem)
                                text.Span("   ");

                            text.Span(resume.PhoneNumber).FontSize(11);
                            hasPreviousItem = true;
                        }

                        foreach (var link in resume.PersonalLinks)
                        {
                            if (hasPreviousItem)
                                text.Span("   ")
                                .FontSize(11); ;

                            text.Hyperlink(link.DisplayName, link.Url)
                                .Underline()
                                .FontColor(Color.FromHex("#467886"))
                                .FontSize(11); ;

                            hasPreviousItem = true;
                        }
                    });
            });
    }

    private static void ComposeSummary(ColumnDescriptor column, ResumeResponse resume)
    {
        if (string.IsNullOrWhiteSpace(resume.Summary))
            return;

        SectionHeader(column, "SUMMARY", resume.Appearance);

        column.Item()
            .PaddingTop(SectionContentSpacing)
            .Text(resume.Summary);
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
        SectionHeader(column, "PROFESSIONAL EXPERIENCE", resume.Appearance);

        var index = 0;
        foreach (var company in resume.Experiences.Where(exp => exp.Type is ExperienceType.Employment or ExperienceType.Contract or ExperienceType.Freelance))
        {
            column.Item()
                .PaddingTop(index == 0 ? SectionContentSpacing : CompanySpacing)
                .SemanticSection()
                .Column(companyColumn =>
                {
                    var date = company.Ended is DateOnly ended
                        ? $"{company.Started:MMM yyyy} - {company.Ended:MMM yyyy}"
                        : $"{company.Started:MMM yyyy} - Present";

                    TwoColumn(companyColumn, company.Organization, date, bold: true, isHeading: true);
                    TwoColumn(companyColumn, company.Title, company.Location ?? "Remote");

                    if (company.Bullets.Count > 0)
                    {
                        companyColumn
                            .Item()
                            .PaddingTop(BulletTopSpacing)
                            .Column(bullets =>
                            {
                                bullets.Spacing(BulletSpacing);

                                foreach (var bullet in company.Bullets)
                                {
                                    bullets
                                        .Item()
                                        .SemanticListItem()
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
        foreach(var education in resume.Education.Where(edu => edu.Type == EducationType.Degree))
        {
            column
                .Item()
                .PaddingTop(index == 0 ? SectionContentSpacing : CompanySpacing)
                .Column(educationColumn =>
                {
                    var date = education.Ended is DateOnly ended
                        ? ended.ToString("MMM yyyy")
                        : $"{education.Started:MMM yyyy} - Present";

                    TwoColumn(educationColumn, education.InstitutionName, date, true, isHeading: true);

                    TwoColumn(educationColumn, education.Qualification, education.FieldOfStudy);
                });
            index++;
        }
    }

    private static void ComposeCertifcations(ColumnDescriptor column, ResumeResponse resume)
    {
        SectionHeader(column, "CERTIFICATIONS", resume.Appearance);

        var index = 0;
        foreach (var education in resume.Education.Where(edu => edu.Type == EducationType.Certificate || edu.Type == EducationType.Certification))
        {
            column
                .Item()
                .PaddingTop(index == 0 ? SectionContentSpacing : CompanySpacing)
                .Column(educationColumn =>
                {
                    var date = education.Ended is DateOnly ended
                        ? ended.ToString("MMM yyyy")
                        : $"{education.Started:MMM yyyy} - Present";

                    TwoColumn(educationColumn, education.Qualification, date, true, isHeading: true);

                    if (!resume.Appearance.HideCertificationIssuer)
                    {
                        OneColumn(educationColumn, education.InstitutionName);
                    }

                });
            index++;
        }
    }

    private static void ComposeProjects(ColumnDescriptor column, ResumeResponse resume)
    {
        SectionHeader(column, "PROJECTS", resume.Appearance);

        var index = 0;
        foreach (var project in resume.Experiences.Where(exp => exp.Type is ExperienceType.PersonalProject
                                                                            or ExperienceType.OpenSourceProject
                                                                            or ExperienceType.AcademicProject))
        {
            column
                .Item()
                .PaddingTop(index == 0 ? SectionContentSpacing : CompanySpacing)
                .Column(projectColumn =>
                {
                    var date = project.Ended is DateOnly ended
                        ? ended.ToString("MMM yyyy")
                        : $"{project.Started:MMM yyyy} - Present";

                    TwoColumn(projectColumn, project.Title, date, bold: true, isHeading: true);

                    if (!string.IsNullOrWhiteSpace(project.PrimarySummary?.Value))
                    {
                        projectColumn
                            .Item()
                            .PaddingTop(SectionContentSpacing)
                            .PaddingLeft(ProjectIndent)
                            .Text(project.PrimarySummary.Value);
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
            .SemanticHeading2()
            .Text(title)
            .FontSize(appearance.SectionHeaderFontSize)
            .FontColor(Color.FromHex(appearance.FontColor))
            .SemiBold();
    }

    private static void TwoColumn(ColumnDescriptor column,string? left,string? right,bool bold = false, bool isHeading = false)
    {
        column
            .Item()
            .Row(row =>
            {
                var leftContainer = row.RelativeItem(3);

                if (isHeading)
                    leftContainer = leftContainer.SemanticHeading3();

                var leftText = leftContainer.Text(left ?? string.Empty);

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

    private static void OneColumn(ColumnDescriptor column, string? text,bool bold = false, bool isHeading = false, bool alignRight = false)
    {
        var container = column.Item();

        if (isHeading)
            container = container.SemanticHeading3();

        var textDescriptor = container.Text(text ?? string.Empty);

        if (bold)
            textDescriptor.Bold();

        if (alignRight)
            textDescriptor.AlignRight();
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
