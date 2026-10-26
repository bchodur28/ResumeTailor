using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeExperienceTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LibrarySummary_ExperienceId",
                table: "LibrarySummary");

            migrationBuilder.CreateTable(
                name: "ResumeExperience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryExperienceId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResumeSummaryId = table.Column<int>(type: "INTEGER", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeExperience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeExperience_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeBullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeExperienceId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibraryBulletId = table.Column<int>(type: "INTEGER", nullable: false),
                    Alternative = table.Column<string>(type: "TEXT", nullable: true),
                    Reasoning = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeBullet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeBullet_ResumeExperience_ResumeExperienceId",
                        column: x => x.ResumeExperienceId,
                        principalTable: "ResumeExperience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LibrarySummary_ExperienceId",
                table: "LibrarySummary",
                column: "ExperienceId",
                unique: true,
                filter: "\"IsPrimary\" = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeBullet_ResumeExperienceId",
                table: "ResumeBullet",
                column: "ResumeExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeExperience_ResumeId",
                table: "ResumeExperience",
                column: "ResumeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResumeBullet");

            migrationBuilder.DropTable(
                name: "ResumeExperience");

            migrationBuilder.DropIndex(
                name: "IX_LibrarySummary_ExperienceId",
                table: "LibrarySummary");

            migrationBuilder.CreateIndex(
                name: "IX_LibrarySummary_ExperienceId",
                table: "LibrarySummary",
                column: "ExperienceId");
        }
    }
}
