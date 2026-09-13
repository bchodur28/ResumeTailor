using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Auth0UserId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Started = table.Column<DateOnly>(type: "date", nullable: false),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    GenerateBullets = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxGeneratedBulletCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    SchoolName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Degree = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Major = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Started = table.Column<DateOnly>(type: "date", nullable: false),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    UseForResume = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    JobName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    WorkStyle = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    JobDescription = table.Column<string>(type: "TEXT", nullable: true),
                    JobUrl = table.Column<string>(type: "TEXT", nullable: true),
                    AppliedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Started = table.Column<DateOnly>(type: "date", nullable: false),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    TechStack = table.Column<string>(type: "TEXT", nullable: true),
                    Link = table.Column<string>(type: "TEXT", nullable: true),
                    UseForResume = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteExtractionDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SiteName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Hostname = table.Column<string>(type: "TEXT", nullable: false),
                    PathPattern = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteExtractionDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalLink",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                name: "Title",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                name: "Bullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    AiScore = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bullet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bullet_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedResume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    JobApplicationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedResume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedResume_JobApplication_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FieldExtractionDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SiteExtractionDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DisplayLabel = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ExtractionType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AttributeName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                name: "ResumeAiAnalysis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GeneratedResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAiAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeAiAnalysis_GeneratedResume_GeneratedResumeId",
                        column: x => x.GeneratedResumeId,
                        principalTable: "GeneratedResume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeAiMetaData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GeneratedResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    InputTokens = table.Column<int>(type: "INTEGER", nullable: false),
                    OutputTokens = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalTokens = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAiMetaData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeAiMetaData_GeneratedResume_GeneratedResumeId",
                        column: x => x.GeneratedResumeId,
                        principalTable: "GeneratedResume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeCompanySelection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GeneratedResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeCompanySelection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeCompanySelection_GeneratedResume_GeneratedResumeId",
                        column: x => x.GeneratedResumeId,
                        principalTable: "GeneratedResume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeEducationSelection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GeneratedResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    EducationId = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeEducationSelection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeEducationSelection_GeneratedResume_GeneratedResumeId",
                        column: x => x.GeneratedResumeId,
                        principalTable: "GeneratedResume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeProjectSelection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GeneratedResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeProjectSelection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeProjectSelection_GeneratedResume_GeneratedResumeId",
                        column: x => x.GeneratedResumeId,
                        principalTable: "GeneratedResume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldPattern",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FieldExtractionDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScopePattern = table.Column<string>(type: "TEXT", nullable: true),
                    MatchPattern = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldExtractionDefinitionId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_FieldPattern_FieldExtractionDefinition_FieldExtractionDefinitionId1",
                        column: x => x.FieldExtractionDefinitionId1,
                        principalTable: "FieldExtractionDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeAiInsight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeAiAnalysisId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ResumeAiAnalysisId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    ResumeAiAnalysisId2 = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_ResumeAiInsight_ResumeAiAnalysis_ResumeAiAnalysisId1",
                        column: x => x.ResumeAiAnalysisId1,
                        principalTable: "ResumeAiAnalysis",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResumeAiInsight_ResumeAiAnalysis_ResumeAiAnalysisId2",
                        column: x => x.ResumeAiAnalysisId2,
                        principalTable: "ResumeAiAnalysis",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResumeBullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeCompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceBulletId = table.Column<int>(type: "INTEGER", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    AlternativeValue = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeBullet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeBullet_ResumeCompanySelection_ResumeCompanyId",
                        column: x => x.ResumeCompanyId,
                        principalTable: "ResumeCompanySelection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bullet_CompanyId",
                table: "Bullet",
                column: "CompanyId");

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
                name: "IX_FieldPattern_FieldExtractionDefinitionId1",
                table: "FieldPattern",
                column: "FieldExtractionDefinitionId1");

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedResume_JobApplicationId",
                table: "GeneratedResume",
                column: "JobApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalLink_AccountId",
                table: "PersonalLink",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiAnalysis_GeneratedResumeId",
                table: "ResumeAiAnalysis",
                column: "GeneratedResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId1",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId1");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId2",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId2");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiMetaData_GeneratedResumeId",
                table: "ResumeAiMetaData",
                column: "GeneratedResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeBullet_ResumeCompanyId",
                table: "ResumeBullet",
                column: "ResumeCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeCompanySelection_GeneratedResumeId",
                table: "ResumeCompanySelection",
                column: "GeneratedResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeEducationSelection_GeneratedResumeId",
                table: "ResumeEducationSelection",
                column: "GeneratedResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeProjectSelection_GeneratedResumeId",
                table: "ResumeProjectSelection",
                column: "GeneratedResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteExtractionDefinition_Hostname_PathPattern_Version",
                table: "SiteExtractionDefinition",
                columns: new[] { "Hostname", "PathPattern", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Title_AccountId",
                table: "Title",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bullet");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "FieldPattern");

            migrationBuilder.DropTable(
                name: "PersonalLink");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "ResumeAiInsight");

            migrationBuilder.DropTable(
                name: "ResumeAiMetaData");

            migrationBuilder.DropTable(
                name: "ResumeBullet");

            migrationBuilder.DropTable(
                name: "ResumeEducationSelection");

            migrationBuilder.DropTable(
                name: "ResumeProjectSelection");

            migrationBuilder.DropTable(
                name: "Title");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "FieldExtractionDefinition");

            migrationBuilder.DropTable(
                name: "ResumeAiAnalysis");

            migrationBuilder.DropTable(
                name: "ResumeCompanySelection");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "SiteExtractionDefinition");

            migrationBuilder.DropTable(
                name: "GeneratedResume");

            migrationBuilder.DropTable(
                name: "JobApplication");
        }
    }
}
