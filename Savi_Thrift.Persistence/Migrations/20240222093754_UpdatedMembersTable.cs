using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    public partial class UpdatedMembersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasCollected",
                table: "GroupSavingsMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasCollected",
                table: "GroupSavingsMembers");
        }
    }
}
