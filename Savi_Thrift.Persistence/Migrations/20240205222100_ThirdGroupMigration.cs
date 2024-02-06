using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    public partial class ThirdGroupMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupTransactions_Groups_GroupId",
                table: "GroupTransactions");

            migrationBuilder.DropTable(
                name: "AppUserGroup");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_GroupTransactions_GroupId",
                table: "GroupTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSavingsFundings",
                table: "GroupSavingsFundings");

            migrationBuilder.RenameTable(
                name: "GroupSavingsFundings",
                newName: "GroupSavingsFunding");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "GroupTransactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "GroupSavingsId",
                table: "GroupSavingsMembers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "GroupSavings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GroupSavingsId",
                table: "GroupSavingsFunding",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSavingsFunding",
                table: "GroupSavingsFunding",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsMembers_GroupSavingsId",
                table: "GroupSavingsMembers",
                column: "GroupSavingsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavings_AppUserId",
                table: "GroupSavings",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSavingsFunding_GroupSavingsId",
                table: "GroupSavingsFunding",
                column: "GroupSavingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavings_AspNetUsers_AppUserId",
                table: "GroupSavings",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsFunding_GroupSavings_GroupSavingsId",
                table: "GroupSavingsFunding",
                column: "GroupSavingsId",
                principalTable: "GroupSavings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSavingsMembers_GroupSavings_GroupSavingsId",
                table: "GroupSavingsMembers",
                column: "GroupSavingsId",
                principalTable: "GroupSavings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavings_AspNetUsers_AppUserId",
                table: "GroupSavings");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsFunding_GroupSavings_GroupSavingsId",
                table: "GroupSavingsFunding");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSavingsMembers_GroupSavings_GroupSavingsId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupSavingsMembers_GroupSavingsId",
                table: "GroupSavingsMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupSavings_AppUserId",
                table: "GroupSavings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSavingsFunding",
                table: "GroupSavingsFunding");

            migrationBuilder.DropIndex(
                name: "IX_GroupSavingsFunding_GroupSavingsId",
                table: "GroupSavingsFunding");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "GroupSavings");

            migrationBuilder.RenameTable(
                name: "GroupSavingsFunding",
                newName: "GroupSavingsFundings");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "GroupTransactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GroupSavingsId",
                table: "GroupSavingsMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "GroupSavingsId",
                table: "GroupSavingsFundings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSavingsFundings",
                table: "GroupSavingsFundings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AvailableSlots = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CashoutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContributionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMonths = table.Column<int>(type: "int", nullable: false),
                    EstimatedCollection = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    MaxNumberOfParticipants = table.Column<int>(type: "int", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    SavingFrequency = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Terms = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserGroup",
                columns: table => new
                {
                    GroupsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserGroup", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserGroup_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserGroup_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTransactions_GroupId",
                table: "GroupTransactions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserGroup_UsersId",
                table: "AppUserGroup",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupTransactions_Groups_GroupId",
                table: "GroupTransactions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
