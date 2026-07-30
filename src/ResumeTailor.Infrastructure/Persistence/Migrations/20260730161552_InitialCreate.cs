using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteExtractionDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SiteName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Hostname = table.Column<string>(type: "TEXT", nullable: false),
                    PathPattern = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteExtractionDefinition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FieldExtractionDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SiteExtractionDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldName = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayLabel = table.Column<string>(type: "TEXT", nullable: false),
                    ExtractionType = table.Column<int>(type: "INTEGER", nullable: false),
                    AttributeName = table.Column<string>(type: "TEXT", nullable: true),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldExtractionDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldExtractionDefinitions_SiteExtractionDefinition_SiteExtractionDefinitionId",
                        column: x => x.SiteExtractionDefinitionId,
                        principalTable: "SiteExtractionDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FieldSelector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FieldExtractionDefinitionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Selector = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldSelector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FieldSelector_FieldExtractionDefinitions_FieldExtractionDefinitionId",
                        column: x => x.FieldExtractionDefinitionId,
                        principalTable: "FieldExtractionDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldExtractionDefinitions_SiteExtractionDefinitionId",
                table: "FieldExtractionDefinitions",
                column: "SiteExtractionDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldSelector_FieldExtractionDefinitionId_Priority",
                table: "FieldSelector",
                columns: new[] { "FieldExtractionDefinitionId", "Priority" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FieldSelector_FieldExtractionDefinitionId_Selector",
                table: "FieldSelector",
                columns: new[] { "FieldExtractionDefinitionId", "Selector" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteExtractionDefinition_Version",
                table: "SiteExtractionDefinition",
                column: "Version",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FieldSelector");

            migrationBuilder.DropTable(
                name: "FieldExtractionDefinitions");

            migrationBuilder.DropTable(
                name: "SiteExtractionDefinition");
        }
    }
}
