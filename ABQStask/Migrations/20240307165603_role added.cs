using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABQStask.Migrations
{
    public partial class roleadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roll");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("0d559439-9562-4186-9538-0f37ba3df7d2"));

            migrationBuilder.RenameColumn(
                name: "RollId",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RollName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RollName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "SubAdmin" },
                    { 3, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Name", "Password", "Phone", "RoleId", "isDeleted" },
                values: new object[] { new Guid("cc165651-aa28-4867-a40c-0b0045019f53"), "admin999@gmail.com", "Admin1", "Admin1", "9874563210", 1, false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("cc165651-aa28-4867-a40c-0b0045019f53"));

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "RollId");

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
    }
}
