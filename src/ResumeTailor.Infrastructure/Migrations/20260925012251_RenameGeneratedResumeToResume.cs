using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameGeneratedResumeToResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResumeAiAnalysis_GeneratedResume_GeneratedResumeId",
                table: "ResumeAiAnalysis");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeAiMetaData_GeneratedResume_GeneratedResumeId",
                table: "ResumeAiMetaData");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeCompanySelection_GeneratedResume_GeneratedResumeId",
                table: "ResumeCompanySelection");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeEducationSelection_GeneratedResume_GeneratedResumeId",
                table: "ResumeEducationSelection");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeProjectSelection_GeneratedResume_GeneratedResumeId",
                table: "ResumeProjectSelection");

            migrationBuilder.DropTable(
                name: "GeneratedResume");

            migrationBuilder.DropTable(
                name: "ResumeBullet");

            migrationBuilder.RenameColumn(
                name: "GeneratedResumeId",
                table: "ResumeProjectSelection",
                newName: "ResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeProjectSelection_GeneratedResumeId",
                table: "ResumeProjectSelection",
                newName: "IX_ResumeProjectSelection_ResumeId");

            migrationBuilder.RenameColumn(
                name: "GeneratedResumeId",
                table: "ResumeEducationSelection",
                newName: "ResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeEducationSelection_GeneratedResumeId",
                table: "ResumeEducationSelection",
                newName: "IX_ResumeEducationSelection_ResumeId");

            migrationBuilder.RenameColumn(
                name: "GeneratedResumeId",
                table: "ResumeCompanySelection",
                newName: "ResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeCompanySelection_GeneratedResumeId",
                table: "ResumeCompanySelection",
                newName: "IX_ResumeCompanySelection_ResumeId");

            migrationBuilder.RenameColumn(
                name: "GeneratedResumeId",
                table: "ResumeAiMetaData",
                newName: "ResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeAiMetaData_GeneratedResumeId",
                table: "ResumeAiMetaData",
                newName: "IX_ResumeAiMetaData_ResumeId");

            migrationBuilder.RenameColumn(
                name: "GeneratedResumeId",
                table: "ResumeAiAnalysis",
                newName: "ResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeAiAnalysis_GeneratedResumeId",
                table: "ResumeAiAnalysis",
                newName: "IX_ResumeAiAnalysis_ResumeId");

            migrationBuilder.CreateTable(
                name: "Resume",
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
                    table.PrimaryKey("PK_Resume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resume_JobApplication_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ResumeCompanyBullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceBulletId = table.Column<int>(type: "INTEGER", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    AlternativeValue = table.Column<string>(type: "TEXT", nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeCompanyBullet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeCompanyBullet_ResumeCompanySelection_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "ResumeCompanySelection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resume_JobApplicationId",
                table: "Resume",
                column: "JobApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeCompanyBullet_CompanyId",
                table: "ResumeCompanyBullet",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeAiAnalysis_Resume_ResumeId",
                table: "ResumeAiAnalysis",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeAiMetaData_Resume_ResumeId",
                table: "ResumeAiMetaData",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeCompanySelection_Resume_ResumeId",
                table: "ResumeCompanySelection",
                column: "ResumeId",
                principalTable: "Resume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeEducationSelection_Resume_ResumeId",
                table: "ResumeEducationSelection",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResumeAiAnalysis_Resume_ResumeId",
                table: "ResumeAiAnalysis");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeAiMetaData_Resume_ResumeId",
                table: "ResumeAiMetaData");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeCompanySelection_Resume_ResumeId",
                table: "ResumeCompanySelection");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeEducationSelection_Resume_ResumeId",
                table: "ResumeEducationSelection");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeProjectSelection_Resume_ResumeId",
                table: "ResumeProjectSelection");

            migrationBuilder.DropTable(
                name: "Resume");

            migrationBuilder.DropTable(
                name: "ResumeCompanyBullet");

            migrationBuilder.RenameColumn(
                name: "ResumeId",
                table: "ResumeProjectSelection",
                newName: "GeneratedResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeProjectSelection_ResumeId",
                table: "ResumeProjectSelection",
                newName: "IX_ResumeProjectSelection_GeneratedResumeId");

            migrationBuilder.RenameColumn(
                name: "ResumeId",
                table: "ResumeEducationSelection",
                newName: "GeneratedResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeEducationSelection_ResumeId",
                table: "ResumeEducationSelection",
                newName: "IX_ResumeEducationSelection_GeneratedResumeId");

            migrationBuilder.RenameColumn(
                name: "ResumeId",
                table: "ResumeCompanySelection",
                newName: "GeneratedResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeCompanySelection_ResumeId",
                table: "ResumeCompanySelection",
                newName: "IX_ResumeCompanySelection_GeneratedResumeId");

            migrationBuilder.RenameColumn(
                name: "ResumeId",
                table: "ResumeAiMetaData",
                newName: "GeneratedResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeAiMetaData_ResumeId",
                table: "ResumeAiMetaData",
                newName: "IX_ResumeAiMetaData_GeneratedResumeId");

            migrationBuilder.RenameColumn(
                name: "ResumeId",
                table: "ResumeAiAnalysis",
                newName: "GeneratedResumeId");

            migrationBuilder.RenameIndex(
                name: "IX_ResumeAiAnalysis_ResumeId",
                table: "ResumeAiAnalysis",
                newName: "IX_ResumeAiAnalysis_GeneratedResumeId");

            migrationBuilder.CreateTable(
                name: "GeneratedResume",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    JobApplicationId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "ResumeBullet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlternativeValue = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ResumeCompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    SourceBulletId = table.Column<int>(type: "INTEGER", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "IX_GeneratedResume_JobApplicationId",
                table: "GeneratedResume",
                column: "JobApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeBullet_ResumeCompanyId",
                table: "ResumeBullet",
                column: "ResumeCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeAiAnalysis_GeneratedResume_GeneratedResumeId",
                table: "ResumeAiAnalysis",
                column: "GeneratedResumeId",
                principalTable: "GeneratedResume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeAiMetaData_GeneratedResume_GeneratedResumeId",
                table: "ResumeAiMetaData",
                column: "GeneratedResumeId",
                principalTable: "GeneratedResume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeCompanySelection_GeneratedResume_GeneratedResumeId",
                table: "ResumeCompanySelection",
                column: "GeneratedResumeId",
                principalTable: "GeneratedResume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeEducationSelection_GeneratedResume_GeneratedResumeId",
                table: "ResumeEducationSelection",
                column: "GeneratedResumeId",
                principalTable: "GeneratedResume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeProjectSelection_GeneratedResume_GeneratedResumeId",
                table: "ResumeProjectSelection",
                column: "GeneratedResumeId",
                principalTable: "GeneratedResume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
