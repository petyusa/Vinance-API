using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinance.Data.Migrations
{
    public partial class RemovedCategoryFromTransferEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Categories_CategoryId",
                schema: "Vinance",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_CategoryId",
                schema: "Vinance",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "Vinance",
                table: "Transfers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "Vinance",
                table: "Transfers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_CategoryId",
                schema: "Vinance",
                table: "Transfers",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Categories_CategoryId",
                schema: "Vinance",
                table: "Transfers",
                column: "CategoryId",
                principalSchema: "Vinance",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
