using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "UserTransactions",
                newName: "ActionId");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "UserTransactions",
                newName: "WalletNumber");

            migrationBuilder.AddColumn<string>(
                name: "SavingsId",
                table: "UserTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SavingsId",
                table: "UserTransactions");

            migrationBuilder.RenameColumn(
                name: "WalletNumber",
                table: "UserTransactions",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "ActionId",
                table: "UserTransactions",
                newName: "TransactionType");
        }
    }
}
