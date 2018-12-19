using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinance.Data.Migrations
{
    public partial class AddedIsSavingAndIsActiveToAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Vinance",
                table: "Accounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSaving",
                schema: "Vinance",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Vinance",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsSaving",
                schema: "Vinance",
                table: "Accounts");
        }
    }
}
