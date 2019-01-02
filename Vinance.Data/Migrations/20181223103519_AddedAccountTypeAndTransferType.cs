using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinance.Data.Migrations
{
    public partial class AddedAccountTypeAndTransferType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSaving",
                schema: "Vinance",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "TransferType",
                schema: "Vinance",
                table: "Transfers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                schema: "Vinance",
                table: "Accounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferType",
                schema: "Vinance",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "AccountType",
                schema: "Vinance",
                table: "Accounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsSaving",
                schema: "Vinance",
                table: "Accounts",
                nullable: false,
                defaultValue: false);
        }
    }
}
