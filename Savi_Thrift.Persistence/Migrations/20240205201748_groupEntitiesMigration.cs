using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Savi_Thrift.Persistence.Migrations
{
    public partial class groupEntitiesMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupSavings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContributionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpectedStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    MemberCount = table.Column<int>(type: "int", nullable: false),
                    RunTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurposeAndGoal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupStatus = table.Column<int>(type: "int", nullable: false),
                    SafePortraitImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SafeLandScapeImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextRunTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSavings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupSavingsFundings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupSavingsId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSavingsFundings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupSavingsMembers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupSavingsId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsGroupOwner = table.Column<bool>(type: "bit", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSavingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSavingsMembers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupSavings");

            migrationBuilder.DropTable(
                name: "GroupSavingsFundings");

            migrationBuilder.DropTable(
                name: "GroupSavingsMembers");
        }
    }
}
