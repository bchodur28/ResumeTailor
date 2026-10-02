using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddResumeAppearanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResumeAppearance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    TitleFontSize = table.Column<int>(type: "INTEGER", nullable: false),
                    SectionHeaderFontSize = table.Column<int>(type: "INTEGER", nullable: false),
                    MainBodyFontSize = table.Column<int>(type: "INTEGER", nullable: false),
                    FontFamily = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FontColor = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    TopHeaderAlignment = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeAppearance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeAppearance_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResumeAppearance_ResumeId",
                table: "ResumeAppearance",
                column: "ResumeId",
                unique: true);

            migrationBuilder.Sql("""
                INSERT INTO ResumeAppearance
                (
                    ResumeId,
                    TitleFontSize,
                    SectionHeaderFontSize,
                    MainBodyFontSize,
                    FontFamily,
                    FontColor,
                    TopHeaderAlignment,
                    CreatedDate,
                    UpdatedDate
                )
                SELECT
                    Id,
                    18,
                    14,
                    10,
                    'calibri, sans-serif',
                    '#1a2e5b',
                    'Center',
                    CURRENT_TIMESTAMP,
                    NULL
                FROM Resume;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResumeAppearance");
        }
    }
}
