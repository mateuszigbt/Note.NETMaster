using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Noteapp.Migrations
{
    /// <inheritdoc />
    public partial class changednote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0a21adf2-a7b0-4d13-b35d-a87e9333ca9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a8e95e5-70b6-4edd-93f3-2572d96a318a");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5603ba42-4d5d-451e-a941-8eae29e160e3", null, "Admin", "ADMIN" },
                    { "9a27c752-8baf-4157-9094-65c66c1ff1c6", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5603ba42-4d5d-451e-a941-8eae29e160e3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a27c752-8baf-4157-9094-65c66c1ff1c6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0a21adf2-a7b0-4d13-b35d-a87e9333ca9c", null, "User", "USER" },
                    { "4a8e95e5-70b6-4edd-93f3-2572d96a318a", null, "Admin", "ADMIN" }
                });
        }
    }
}
