using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    public partial class GroupTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupSavingsFunding");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "GroupTransactions");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "GroupTransactions");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "GroupTransactions",
                newName: "ActionId");

            migrationBuilder.AddColumn<string>(
                name: "GroupSavingsId",
                table: "GroupTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupTransactions_GroupSavingsId",
                table: "GroupTransactions",
                column: "GroupSavingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTransactions_GroupSavings_GroupSavingsId",
                table: "GroupTransactions",
                column: "GroupSavingsId",
                principalTable: "GroupSavings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTransactions_GroupSavings_GroupSavingsId",
                table: "GroupTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GroupTransactions_GroupSavingsId",
                table: "GroupTransactions");

            migrationBuilder.DropColumn(
                name: "GroupSavingsId",
                table: "GroupTransactions");

            migrationBuilder.RenameColumn(
                name: "ActionId",
                table: "GroupTransactions",
                newName: "Reference");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GroupTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupId",
                table: "GroupTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "GroupTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupSavingsFunding",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ActionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GroupSavingsId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSavingsFunding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSavingsFunding_GroupSavings_GroupSavingsId",
                        column: x => x.GroupSavingsId,
                        principalTable: "GroupSavings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsFunding_GroupSavingsId",
                table: "GroupSavingsFunding",
                column: "GroupSavingsId");
        }
    }
}
