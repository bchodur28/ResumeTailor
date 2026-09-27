using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJobPostingAndJobApplicationTrackingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resume_JobApplication_JobApplicationId",
                table: "Resume");

            migrationBuilder.DropTable(
                name: "JobApplication");

            migrationBuilder.DropIndex(
                name: "IX_Resume_JobApplicationId",
                table: "Resume");

            migrationBuilder.DropColumn(
                name: "JobApplicationId",
                table: "Resume");

            migrationBuilder.AddColumn<bool>(
                name: "HasEditBullets",
                table: "Resume",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ResumeApplicationTracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "date", nullable: false),
                    Applied = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Interviewed = table.Column<DateOnly>(type: "date", nullable: true),
                    OfferReceived = table.Column<DateOnly>(type: "date", nullable: true),
                    OfferAccepted = table.Column<DateOnly>(type: "date", nullable: true),
                    Rejected = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeApplicationTracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeApplicationTracking_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResumeJobPosting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ResumeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    JobTitle = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    WorkStyle = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: true),
                    SalaryMin = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryMax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryPeriod = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: true),
                    SalaryCurrency = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResumeJobPosting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResumeJobPosting_Resume_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResumeApplicationTracking_ResumeId",
                table: "ResumeApplicationTracking",
                column: "ResumeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeJobPosting_ResumeId",
                table: "ResumeJobPosting",
                column: "ResumeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResumeApplicationTracking");

            migrationBuilder.DropTable(
                name: "ResumeJobPosting");

            migrationBuilder.DropColumn(
                name: "HasEditBullets",
                table: "Resume");

            migrationBuilder.AddColumn<int>(
                name: "JobApplicationId",
                table: "Resume",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    AppliedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    JobDescription = table.Column<string>(type: "TEXT", nullable: true),
                    JobName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    JobUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WorkStyle = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplication", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resume_JobApplicationId",
                table: "Resume",
                column: "JobApplicationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Resume_JobApplication_JobApplicationId",
                table: "Resume",
                column: "JobApplicationId",
                principalTable: "JobApplication",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
