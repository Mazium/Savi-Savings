using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    public partial class EntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "KYCs",
                newName: "AppUserId");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "UserTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Savings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "KYCs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "KYCs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "KYCs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "GroupTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FundFrequency",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_AppUserId",
                table: "UserTransactions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_AppUserId",
                table: "Savings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTransactions_AppUserId",
                table: "GroupTransactions",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTransactions_AspNetUsers_AppUserId",
                table: "GroupTransactions",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Savings_AspNetUsers_AppUserId",
                table: "Savings",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTransactions_AspNetUsers_AppUserId",
                table: "UserTransactions",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTransactions_AspNetUsers_AppUserId",
                table: "GroupTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Savings_AspNetUsers_AppUserId",
                table: "Savings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTransactions_AspNetUsers_AppUserId",
                table: "UserTransactions");

            migrationBuilder.DropIndex(
                name: "IX_UserTransactions_AppUserId",
                table: "UserTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Savings_AppUserId",
                table: "Savings");

            migrationBuilder.DropIndex(
                name: "IX_GroupTransactions_AppUserId",
                table: "GroupTransactions");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserTransactions");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Savings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "KYCs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "KYCs");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "KYCs");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "GroupTransactions");

            migrationBuilder.DropColumn(
                name: "FundFrequency",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "KYCs",
                newName: "UserId");
        }
    }
}
