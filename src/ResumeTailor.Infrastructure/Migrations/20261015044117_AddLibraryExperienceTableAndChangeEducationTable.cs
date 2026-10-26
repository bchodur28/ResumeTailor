using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLibraryExperienceTableAndChangeEducationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.CreateTable(
                name: "LibraryEducation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstitutionName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Qualification = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FieldOfStudy = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Gpa = table.Column<decimal>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Started = table.Column<DateOnly>(type: "date", nullable: false),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    UseForResume = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryEducation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryExperience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Organization = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Started = table.Column<DateOnly>(type: "date", nullable: true),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    LetAiChooseSummary = table.Column<bool>(type: "INTEGER", nullable: false),
                    Link = table.Column<string>(type: "TEXT", nullable: true),
                    TechStack = table.Column<string>(type: "TEXT", nullable: true),
                    LetAiChooseBullets = table.Column<bool>(type: "INTEGER", nullable: false),
                    MaxBulletsToChoose = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryExperience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExperienceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    AiRewrite = table.Column<string>(type: "TEXT", nullable: true),
                    AiOpinion = table.Column<string>(type: "TEXT", nullable: true),
                    AiScore = table.Column<int>(type: "INTEGER", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExperienceId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    AiRewrite = table.Column<string>(type: "TEXT", nullable: true),
                    AiOpinion = table.Column<string>(type: "TEXT", nullable: true),
                    AiScore = table.Column<int>(type: "INTEGER", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBullet_ExperienceId",
                table: "LibraryBullet",
                column: "ExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_LibrarySummary_ExperienceId",
                table: "LibrarySummary",
                column: "ExperienceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LibraryBullet");

            migrationBuilder.DropTable(
                name: "LibraryEducation");

            migrationBuilder.DropTable(
                name: "LibrarySummary");

            migrationBuilder.DropTable(
                name: "LibraryExperience");

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Degree = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Ended = table.Column<DateOnly>(type: "date", nullable: true),
                    Major = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    SchoolName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Started = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UseForResume = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                });
        }
    }
}
