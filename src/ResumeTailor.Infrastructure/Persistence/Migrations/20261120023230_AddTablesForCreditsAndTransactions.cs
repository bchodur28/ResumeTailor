using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesForCreditsAndTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Plan",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CreditWallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditWallet", x => x.Id);
                    table.UniqueConstraint("AK_CreditWallet_AccountId", x => x.AccountId);
                    table.CheckConstraint("CK_CreditWallet_Balance", "[Balance] >= 0");
                    table.ForeignKey(
                        name: "FK_CreditWallet_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreditTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditTransaction", x => x.Id);
                    table.CheckConstraint("CK_CreditTransaction_Amount", "[Amount] <> 0");
                    table.ForeignKey(
                        name: "FK_CreditTransaction_CreditWallet_AccountId",
                        column: x => x.AccountId,
                        principalTable: "CreditWallet",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditTransaction_AccountId_CreatedDate",
                table: "CreditTransaction",
                columns: new[] { "AccountId", "CreatedDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditTransaction");

            migrationBuilder.DropTable(
                name: "CreditWallet");

            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Account");
        }
    }
}
