using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABQStask.Migrations
{
    public partial class rolladded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("d736d4f5-0643-4463-8915-d779abffd711"));

            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RollId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roll",
                columns: table => new
                {
                    RollId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RollName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roll", x => x.RollId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roll",
                columns: new[] { "RollId", "RollName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "SubAdmin" },
                    { 3, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Name", "Password", "Phone", "RollId", "isDeleted" },
                values: new object[] { new Guid("0d559439-9562-4186-9538-0f37ba3df7d2"), "admin999@gmail.com", "Admin1", "Admin1", "9874563210", 1, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roll");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("0d559439-9562-4186-9538-0f37ba3df7d2"));

            migrationBuilder.DropColumn(
                name: "RollId",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "Users",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Name", "Password", "Phone", "isAdmin", "isDeleted" },
                values: new object[] { new Guid("d736d4f5-0643-4463-8915-d779abffd711"), "admin999@gmail.com", "Admin1", "Admin1", "9874563210", true, false });
        }
    }
}
