using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveResumeScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "AiScore",
                table: "Resume",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SourceBulletId",
                table: "ResumeCompanyBullet",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.Sql("""
                UPDATE "Resume"
                SET "AiScore" = (
                    SELECT "Score"
                    FROM "ResumeAiAnalysis"
                    WHERE "ResumeAiAnalysis"."ResumeId" = "Resume"."Id"
                )
                WHERE EXISTS (
                    SELECT 1
                    FROM "ResumeAiAnalysis"
                    WHERE "ResumeAiAnalysis"."ResumeId" = "Resume"."Id"
                );
                """);

                migrationBuilder.DropColumn(
                    name: "Score",
                    table: "ResumeAiAnalysis");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AiScore",
                table: "Resume");

            migrationBuilder.AlterColumn<int>(
                name: "SourceBulletId",
                table: "ResumeCompanyBullet",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "ResumeAiAnalysis",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
