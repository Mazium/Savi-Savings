using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    public partial class balance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Naration",
                table: "WalletFundings",
                newName: "Narration");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Savings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Savings");

            migrationBuilder.RenameColumn(
                name: "Narration",
                table: "WalletFundings",
                newName: "Naration");
        }
    }
}
