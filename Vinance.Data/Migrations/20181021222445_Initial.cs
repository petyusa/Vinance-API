using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vinance.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Vinance");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "Vinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    OpeningBalance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                schema: "Vinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeCategories",
                schema: "Vinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferCategories",
                schema: "Vinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "Vinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(256)", nullable: true),
                    FromId = table.Column<int>(nullable: false),
                    ExpenseCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseCategories_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalSchema: "Vinance",
                        principalTable: "ExpenseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expenses_Accounts_FromId",
                        column: x => x.FromId,
                        principalSchema: "Vinance",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                schema: "Vinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(256)", nullable: true),
                    ToId = table.Column<int>(nullable: false),
                    IncomeCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_IncomeCategories_IncomeCategoryId",
                        column: x => x.IncomeCategoryId,
                        principalSchema: "Vinance",
                        principalTable: "IncomeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incomes_Accounts_ToId",
                        column: x => x.ToId,
                        principalSchema: "Vinance",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transfers",
                schema: "Vinance",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "NVARCHAR(256)", nullable: true),
                    FromId = table.Column<int>(nullable: false),
                    ToId = table.Column<int>(nullable: false),
                    TransferCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Accounts_FromId",
                        column: x => x.FromId,
                        principalSchema: "Vinance",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Accounts_ToId",
                        column: x => x.ToId,
                        principalSchema: "Vinance",
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_TransferCategories_TransferCategoryId",
                        column: x => x.TransferCategoryId,
                        principalSchema: "Vinance",
                        principalTable: "TransferCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Vinance",
                table: "Accounts",
                columns: new[] { "Id", "Name", "OpeningBalance", "UserId" },
                values: new object[,]
                {
                    { 1, "Bankszámla", 0, 0 },
                    { 2, "Megtakarítás", 0, 0 }
                });

            migrationBuilder.InsertData(
                schema: "Vinance",
                table: "ExpenseCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Extra kiadás" },
                    { 2, "Élelmiszer" }
                });

            migrationBuilder.InsertData(
                schema: "Vinance",
                table: "IncomeCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fizetés" },
                    { 2, "Egyéb bevétel" }
                });

            migrationBuilder.InsertData(
                schema: "Vinance",
                table: "TransferCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Kölcsönadás" },
                    { 2, "Levétel" }
                });

            migrationBuilder.InsertData(
                schema: "Vinance",
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Comment", "Date", "ExpenseCategoryId", "FromId" },
                values: new object[,]
                {
                    { 1, 4000, "ez egy komment", new DateTime(2018, 10, 22, 0, 24, 44, 819, DateTimeKind.Local), 1, 1 },
                    { 2, 5000, "ez egy másik komment", new DateTime(2018, 10, 22, 0, 24, 44, 819, DateTimeKind.Local), 2, 2 }
                });

            migrationBuilder.InsertData(
                schema: "Vinance",
                table: "Incomes",
                columns: new[] { "Id", "Amount", "Comment", "Date", "IncomeCategoryId", "ToId" },
                values: new object[,]
                {
                    { 1, 20000, "this is an income comment", new DateTime(2018, 10, 22, 0, 24, 44, 819, DateTimeKind.Local), 1, 1 },
                    { 2, 30000, "this is another income comment", new DateTime(2018, 10, 22, 0, 24, 44, 819, DateTimeKind.Local), 2, 2 }
                });

            migrationBuilder.InsertData(
                schema: "Vinance",
                table: "Transfers",
                columns: new[] { "Id", "Amount", "Comment", "Date", "FromId", "ToId", "TransferCategoryId" },
                values: new object[,]
                {
                    { 1, 20000, "this is a transfer comment", new DateTime(2018, 10, 22, 0, 24, 44, 819, DateTimeKind.Local), 1, 2, 1 },
                    { 2, 20000, "this is another transfer comment", new DateTime(2018, 10, 22, 0, 24, 44, 820, DateTimeKind.Local), 2, 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseCategoryId",
                schema: "Vinance",
                table: "Expenses",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_FromId",
                schema: "Vinance",
                table: "Expenses",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IncomeCategoryId",
                schema: "Vinance",
                table: "Incomes",
                column: "IncomeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_ToId",
                schema: "Vinance",
                table: "Incomes",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_FromId",
                schema: "Vinance",
                table: "Transfers",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToId",
                schema: "Vinance",
                table: "Transfers",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_TransferCategoryId",
                schema: "Vinance",
                table: "Transfers",
                column: "TransferCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "Vinance");

            migrationBuilder.DropTable(
                name: "Incomes",
                schema: "Vinance");

            migrationBuilder.DropTable(
                name: "Transfers",
                schema: "Vinance");

            migrationBuilder.DropTable(
                name: "ExpenseCategories",
                schema: "Vinance");

            migrationBuilder.DropTable(
                name: "IncomeCategories",
                schema: "Vinance");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "Vinance");

            migrationBuilder.DropTable(
                name: "TransferCategories",
                schema: "Vinance");
        }
    }
}
