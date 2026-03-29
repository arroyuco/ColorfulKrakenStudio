using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ColorfulKrakenStudio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tutorials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutorials", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tutorials",
                columns: new[] { "Id", "Author", "CreatedAt", "Duration", "IsFree", "IsPublished", "Plan", "Title" },
                values: new object[,]
                {
                    { 1, "Mamikon", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, true, true, "Free", "Feral Tyranid" },
                    { 2, "David Arroba", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, false, true, "Basic", "Imperial Fist" },
                    { 3, "Kaha Gorska", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 90, false, true, "Pro", "Colorful Wizard" },
                    { 4, "David Arroba", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, true, true, "Free", "Classic Space Marine" },
                    { 5, "Mamikon", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, false, true, "Pro", "Speedpaint Acrylics" },
                    { 6, "Kaha Gorska", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120, false, true, "Pro", "Oil Painting Technique" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Tutorials");
        }
    }
}
