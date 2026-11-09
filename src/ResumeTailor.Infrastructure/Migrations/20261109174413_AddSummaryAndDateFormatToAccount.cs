using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSummaryAndDateFormatToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateFormat",
                table: "Account",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                defaultValue: "MMM_YYYY");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Account",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFormat",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Account");
        }
    }
}
