using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Auth0UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateFormat = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteExtractionDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Hostname = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PathPattern = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteExtractionDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryEducation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    InstitutionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gpa = table.Column<decimal>(type: "decimal(4,3)", precision: 4, scale: 3, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Started = table.Column<DateOnly>(type: "date", nullable: true),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    UseForResume = table.Column<bool>(type: "bit", nullable: false),
                    ArchiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryEducation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryEducation_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryExperience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Started = table.Column<DateOnly>(type: "date", nullable: true),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    LetAiChooseSummary = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TechStack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LetAiChooseBullets = table.Column<bool>(type: "bit", nullable: false),
                    MaxBulletsToChoose = table.Column<int>(type: "int", nullable: true),
                    ArchiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryExperience_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalLink_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AiScore = table.Column<int>(type: "int", nullable: true),
                    AiScoreStaleness = table.Column<int>(type: "int", nullable: false),
                    ResumeSummaryVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiSummaryVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resume_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Title", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Title_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldExtractionDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteExtractionDefinitionId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExtractionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldExtractionDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldExtractionDefinition_SiteExtractionDefinition_SiteExtractionDefinitionId",
                        column: x => x.SiteExtractionDefinitionId,
                        principalTable: "SiteExtractionDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperienceId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AiRewrite = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AiOpinion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AiScore = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBullet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryBullet_LibraryExperience_ExperienceId",
                        column: x => x.ExperienceId,
                        principalTable: "LibraryExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LibrarySummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperienceId = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    AiRewrite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiOpinion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AiScore = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibrarySummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibrarySummary_LibraryExperience_ExperienceId",
                        column: x => x.ExperienceId,
                        principalTable: "LibraryExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeAiAnalysis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAiAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeAiAnalysis_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeAiMetaData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputTokens = table.Column<int>(type: "int", nullable: false),
                    OutputTokens = table.Column<int>(type: "int", nullable: false),
                    TotalTokens = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAiMetaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeAiMetaData_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeAppearance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    TitleFontSize = table.Column<int>(type: "int", nullable: false),
                    SectionHeaderFontSize = table.Column<int>(type: "int", nullable: false),
                    MainBodyFontSize = table.Column<int>(type: "int", nullable: false),
                    FontFamily = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FontColor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TopHeaderAlignment = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HideCertificationIssuer = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAppearance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeAppearance_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeApplicationTracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Applied = table.Column<DateOnly>(type: "date", nullable: true),
                    Interviewed = table.Column<DateOnly>(type: "date", nullable: true),
                    OfferReceived = table.Column<DateOnly>(type: "date", nullable: true),
                    OfferAccepted = table.Column<DateOnly>(type: "date", nullable: true),
                    Rejected = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeApplicationTracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeApplicationTracking_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeEducationSelection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    LibraryEducationId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeEducationSelection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeEducationSelection_LibraryEducation_LibraryEducationId",
                        column: x => x.LibraryEducationId,
                        principalTable: "LibraryEducation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResumeEducationSelection_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeExperience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    LibraryExperienceId = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeExperience_LibraryExperience_LibraryExperienceId",
                        column: x => x.LibraryExperienceId,
                        principalTable: "LibraryExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResumeExperience_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeJobPosting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WorkStyle = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    SalaryMin = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryMax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryPeriod = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    SalaryCurrency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeJobPosting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeJobPosting_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldPattern",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldExtractionDefinitionId = table.Column<int>(type: "int", nullable: false),
                    ScopePattern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatchPattern = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldPattern", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldPattern_FieldExtractionDefinition_FieldExtractionDefinitionId",
                        column: x => x.FieldExtractionDefinitionId,
                        principalTable: "FieldExtractionDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeAiInsight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeAiAnalysisId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAiInsight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeAiInsight_ResumeAiAnalysis_ResumeAiAnalysisId",
                        column: x => x.ResumeAiAnalysisId,
                        principalTable: "ResumeAiAnalysis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeBullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeExperienceId = table.Column<int>(type: "int", nullable: false),
                    LibraryBulletId = table.Column<int>(type: "int", nullable: false),
                    Alternative = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reasoning = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeBullet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeBullet_LibraryBullet_LibraryBulletId",
                        column: x => x.LibraryBulletId,
                        principalTable: "LibraryBullet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResumeBullet_ResumeExperience_ResumeExperienceId",
                        column: x => x.ResumeExperienceId,
                        principalTable: "ResumeExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeExperienceId = table.Column<int>(type: "int", nullable: false),
                    LibrarySummaryId = table.Column<int>(type: "int", nullable: false),
                    WasManuallyChanged = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    Reasoning = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeSummary_LibrarySummary_LibrarySummaryId",
                        column: x => x.LibrarySummaryId,
                        principalTable: "LibrarySummary",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResumeSummary_ResumeExperience_ResumeExperienceId",
                        column: x => x.ResumeExperienceId,
                        principalTable: "ResumeExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldExtractionDefinition_SiteExtractionDefinitionId",
                table: "FieldExtractionDefinition",
                column: "SiteExtractionDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldPattern_FieldExtractionDefinitionId_MatchPattern",
                table: "FieldPattern",
                columns: new[] { "FieldExtractionDefinitionId", "MatchPattern" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldPattern_FieldExtractionDefinitionId_Priority",
                table: "FieldPattern",
                columns: new[] { "FieldExtractionDefinitionId", "Priority" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBullet_ExperienceId",
                table: "LibraryBullet",
                column: "ExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryEducation_AccountId",
                table: "LibraryEducation",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryExperience_AccountId",
                table: "LibraryExperience",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LibrarySummary_ExperienceId",
                table: "LibrarySummary",
                column: "ExperienceId",
                unique: true,
                filter: "[IsPrimary] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalLink_AccountId",
                table: "PersonalLink",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Resume_AccountId",
                table: "Resume",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiAnalysis_ResumeId",
                table: "ResumeAiAnalysis",
                column: "ResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiMetaData_ResumeId",
                table: "ResumeAiMetaData",
                column: "ResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAppearance_ResumeId",
                table: "ResumeAppearance",
                column: "ResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeApplicationTracking_ResumeId",
                table: "ResumeApplicationTracking",
                column: "ResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeBullet_LibraryBulletId",
                table: "ResumeBullet",
                column: "LibraryBulletId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeBullet_ResumeExperienceId",
                table: "ResumeBullet",
                column: "ResumeExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeEducationSelection_LibraryEducationId",
                table: "ResumeEducationSelection",
                column: "LibraryEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeEducationSelection_ResumeId",
                table: "ResumeEducationSelection",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExperience_LibraryExperienceId",
                table: "ResumeExperience",
                column: "LibraryExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExperience_ResumeId",
                table: "ResumeExperience",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeJobPosting_ResumeId",
                table: "ResumeJobPosting",
                column: "ResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeSummary_LibrarySummaryId",
                table: "ResumeSummary",
                column: "LibrarySummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeSummary_ResumeExperienceId",
                table: "ResumeSummary",
                column: "ResumeExperienceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteExtractionDefinition_Hostname_PathPattern_Version",
                table: "SiteExtractionDefinition",
                columns: new[] { "Hostname", "PathPattern", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skill_AccountId",
                table: "Skill",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Title_AccountId",
                table: "Title",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldPattern");

            migrationBuilder.DropTable(
                name: "PersonalLink");

            migrationBuilder.DropTable(
                name: "ResumeAiInsight");

            migrationBuilder.DropTable(
                name: "ResumeAiMetaData");

            migrationBuilder.DropTable(
                name: "ResumeAppearance");

            migrationBuilder.DropTable(
                name: "ResumeApplicationTracking");

            migrationBuilder.DropTable(
                name: "ResumeBullet");

            migrationBuilder.DropTable(
                name: "ResumeEducationSelection");

            migrationBuilder.DropTable(
                name: "ResumeJobPosting");

            migrationBuilder.DropTable(
                name: "ResumeSummary");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "Title");

            migrationBuilder.DropTable(
                name: "FieldExtractionDefinition");

            migrationBuilder.DropTable(
                name: "ResumeAiAnalysis");

            migrationBuilder.DropTable(
                name: "LibraryBullet");

            migrationBuilder.DropTable(
                name: "LibraryEducation");

            migrationBuilder.DropTable(
                name: "LibrarySummary");

            migrationBuilder.DropTable(
                name: "ResumeExperience");

            migrationBuilder.DropTable(
                name: "SiteExtractionDefinition");

            migrationBuilder.DropTable(
                name: "LibraryExperience");

            migrationBuilder.DropTable(
                name: "Resume");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
