using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixSiteExtractionDefinitionVersionIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SiteExtractionDefinition_Version",
                table: "SiteExtractionDefinition");

            migrationBuilder.CreateIndex(
                name: "IX_SiteExtractionDefinition_Hostname_PathPattern_Version",
                table: "SiteExtractionDefinition",
                columns: new[] { "Hostname", "PathPattern", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SiteExtractionDefinition_Hostname_PathPattern_Version",
                table: "SiteExtractionDefinition");

            migrationBuilder.CreateIndex(
                name: "IX_SiteExtractionDefinition_Version",
                table: "SiteExtractionDefinition",
                column: "Version",
                unique: true);
        }
    }
}
