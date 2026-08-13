using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenamePatternToMatchPatternInFieldPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pattern",
                table: "FieldPattern",
                newName: "MatchPattern");

            migrationBuilder.RenameIndex(
                name: "IX_FieldSelector_FieldExtractionDefinitionId_Selector",
                table: "FieldPattern",
                newName: "IX_FieldPattern_FieldExtractionDefinitionId_MatchPattern");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchPattern",
                table: "FieldPattern",
                newName: "Pattern");

            migrationBuilder.RenameIndex(
                name: "IX_FieldSelector_FieldExtractionDefinitionId_Selector",
                table: "FieldPattern",
                newName: "IX_FieldPattern_FieldExtractionDefinitionId_Pattern");
        }
    }
}
