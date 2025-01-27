using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebShop.API.Migrations.WebShopAuthDb
{
    /// <inheritdoc />
    public partial class UpdatedrolesinAuthdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e76d972-56ba-48bb-abef-7b4229483698");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8278b727-d060-4f11-9f25-267c224c4a20");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9867240-9477-4136-9e5a-cca74ce143ba");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83d0736a-f899-4b65-8b43-73be201e670e", "83d0736a-f899-4b65-8b43-73be201e670e", "Manager", "MANAGER" },
                    { "8ec281a0-c9bf-4c16-9346-062afbc90466", "8ec281a0-c9bf-4c16-9346-062afbc90466", "Admin", "ADMIN" },
                    { "b4f150d0-f023-4de5-b9fe-247ff6b6ef87", "b4f150d0-f023-4de5-b9fe-247ff6b6ef87", "RegularUser", "REGULARUSER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83d0736a-f899-4b65-8b43-73be201e670e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ec281a0-c9bf-4c16-9346-062afbc90466");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4f150d0-f023-4de5-b9fe-247ff6b6ef87");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e76d972-56ba-48bb-abef-7b4229483698", "1e76d972-56ba-48bb-abef-7b4229483698", "Manager", "MANAGER" },
                    { "8278b727-d060-4f11-9f25-267c224c4a20", "8278b727-d060-4f11-9f25-267c224c4a20", "RegularUser", "REGULARUSER" },
                    { "e9867240-9477-4136-9e5a-cca74ce143ba", "e9867240-9477-4136-9e5a-cca74ce143ba", "Admin", "ADMIN" }
                });
        }
    }
}
