using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeTailor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletedDateToBullet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Bullet",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResumeCompanyBullet_SourceBulletId",
                table: "ResumeCompanyBullet",
                column: "SourceBulletId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumeCompanyBullet_Bullet_SourceBulletId",
                table: "ResumeCompanyBullet",
                column: "SourceBulletId",
                principalTable: "Bullet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResumeCompanyBullet_Bullet_SourceBulletId",
                table: "ResumeCompanyBullet");

            migrationBuilder.DropIndex(
                name: "IX_ResumeCompanyBullet_SourceBulletId",
                table: "ResumeCompanyBullet");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Bullet");
        }
    }
}
