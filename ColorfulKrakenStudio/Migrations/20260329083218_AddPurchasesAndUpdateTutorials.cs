using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColorfulKrakenStudio.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchasesAndUpdateTutorials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Tutorials");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Tutorials",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TutorialId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Tutorials_TutorialId",
                        column: x => x.TutorialId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 9.99m);

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 9.99m);

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 9.99m);

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 14.99m);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_TutorialId",
                table: "Purchases",
                column: "TutorialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Tutorials");

            migrationBuilder.AddColumn<string>(
                name: "Plan",
                table: "Tutorials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 1,
                column: "Plan",
                value: "Free");

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 2,
                column: "Plan",
                value: "Basic");

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 3,
                column: "Plan",
                value: "Pro");

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 4,
                column: "Plan",
                value: "Free");

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 5,
                column: "Plan",
                value: "Pro");

            migrationBuilder.UpdateData(
                table: "Tutorials",
                keyColumn: "Id",
                keyValue: 6,
                column: "Plan",
                value: "Pro");
        }
    }
}
