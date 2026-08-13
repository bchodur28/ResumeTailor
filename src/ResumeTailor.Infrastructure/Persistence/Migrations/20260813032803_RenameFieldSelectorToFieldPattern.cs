using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameFieldSelectorToFieldPattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "FieldSelector",
                newName: "FieldPattern");

            migrationBuilder.RenameColumn(
                name: "Selector",
                table: "FieldPattern",
                newName: "Pattern");

            migrationBuilder.AddColumn<string>(
                name: "ScopePattern",
                table: "FieldPattern",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScopePattern",
                table: "FieldPattern");

            migrationBuilder.RenameColumn(
                name: "Pattern",
                table: "FieldPattern",
                newName: "Selector");

            migrationBuilder.RenameTable(
                name: "FieldPattern",
                newName: "FieldSelector");
        }
    }
}
