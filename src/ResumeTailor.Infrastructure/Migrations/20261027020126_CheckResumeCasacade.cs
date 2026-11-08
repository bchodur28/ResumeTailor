using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CheckResumeCasacade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResumeCompanySelection_Resume_ResumeId",
                table: "ResumeCompanySelection");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeProjectSelection_Resume_ResumeId",
                table: "ResumeProjectSelection");

            migrationBuilder.DropIndex(
                name: "IX_ResumeProjectSelection_ResumeId",
                table: "ResumeProjectSelection");

            migrationBuilder.DropIndex(
                name: "IX_ResumeCompanySelection_ResumeId",
                table: "ResumeCompanySelection");

            migrationBuilder.DropColumn(
                name: "ResumeSummaryId",
                table: "ResumeExperience");

            migrationBuilder.CreateTable(
                name: "ResumeSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeExperienceId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibrarySummaryId = table.Column<int>(type: "INTEGER", nullable: false),
                    WasManuallyChanged = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Reasoning = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                name: "IX_ResumeExperience_LibraryExperienceId",
                table: "ResumeExperience",
                column: "LibraryExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeEducationSelection_LibraryEducationId",
                table: "ResumeEducationSelection",
                column: "LibraryEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeBullet_LibraryBulletId",
                table: "ResumeBullet",
                column: "LibraryBulletId");

            migrationBuilder.CreateIndex(
                name: "IX_Resume_AccountId",
                table: "Resume",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeSummary_LibrarySummaryId",
                table: "ResumeSummary",
                column: "LibrarySummaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeSummary_ResumeExperienceId",
                table: "ResumeSummary",
                column: "ResumeExperienceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resume_Account_AccountId",
                table: "Resume",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeBullet_LibraryBullet_LibraryBulletId",
                table: "ResumeBullet",
                column: "LibraryBulletId",
                principalTable: "LibraryBullet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeEducationSelection_LibraryEducation_LibraryEducationId",
                table: "ResumeEducationSelection",
                column: "LibraryEducationId",
                principalTable: "LibraryEducation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeExperience_LibraryExperience_LibraryExperienceId",
                table: "ResumeExperience",
                column: "LibraryExperienceId",
                principalTable: "LibraryExperience",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resume_Account_AccountId",
                table: "Resume");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeBullet_LibraryBullet_LibraryBulletId",
                table: "ResumeBullet");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeEducationSelection_LibraryEducation_LibraryEducationId",
                table: "ResumeEducationSelection");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeExperience_LibraryExperience_LibraryExperienceId",
                table: "ResumeExperience");

            migrationBuilder.DropTable(
                name: "ResumeSummary");

            migrationBuilder.DropIndex(
                name: "IX_ResumeExperience_LibraryExperienceId",
                table: "ResumeExperience");

            migrationBuilder.DropIndex(
                name: "IX_ResumeEducationSelection_LibraryEducationId",
                table: "ResumeEducationSelection");

            migrationBuilder.DropIndex(
                name: "IX_ResumeBullet_LibraryBulletId",
                table: "ResumeBullet");

            migrationBuilder.DropIndex(
                name: "IX_Resume_AccountId",
                table: "Resume");

            migrationBuilder.AddColumn<int>(
                name: "ResumeSummaryId",
                table: "ResumeExperience",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeProjectSelection_ResumeId",
                table: "ResumeProjectSelection",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeCompanySelection_ResumeId",
                table: "ResumeCompanySelection",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeCompanySelection_Resume_ResumeId",
                table: "ResumeCompanySelection",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeProjectSelection_Resume_ResumeId",
                table: "ResumeProjectSelection",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
