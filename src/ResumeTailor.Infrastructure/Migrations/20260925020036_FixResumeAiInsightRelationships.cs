using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixResumeAiInsightRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResumeAiInsight_ResumeAiAnalysis_ResumeAiAnalysisId1",
                table: "ResumeAiInsight");

            migrationBuilder.DropForeignKey(
                name: "FK_ResumeAiInsight_ResumeAiAnalysis_ResumeAiAnalysisId2",
                table: "ResumeAiInsight");

            migrationBuilder.DropIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId1",
                table: "ResumeAiInsight");

            migrationBuilder.DropIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId2",
                table: "ResumeAiInsight");

            migrationBuilder.DropColumn(
                name: "ResumeAiAnalysisId1",
                table: "ResumeAiInsight");

            migrationBuilder.DropColumn(
                name: "ResumeAiAnalysisId2",
                table: "ResumeAiInsight");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResumeAiAnalysisId1",
                table: "ResumeAiInsight",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResumeAiAnalysisId2",
                table: "ResumeAiInsight",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId1",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId1");

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAiInsight_ResumeAiAnalysisId2",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId2");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeAiInsight_ResumeAiAnalysis_ResumeAiAnalysisId1",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId1",
                principalTable: "ResumeAiAnalysis",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeAiInsight_ResumeAiAnalysis_ResumeAiAnalysisId2",
                table: "ResumeAiInsight",
                column: "ResumeAiAnalysisId2",
                principalTable: "ResumeAiAnalysis",
                principalColumn: "Id");
        }
    }
}
