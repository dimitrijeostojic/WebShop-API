using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedByinProductclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: new Guid("2f7dd3d3-9097-49de-b750-119d10fe483a"),
                column: "CreatedBy",
                value: "b8815d32-d907-4565-8799-49a5cebcdc4c");

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: new Guid("55acbafe-f9fc-469c-bcac-66955609b9ea"),
                column: "CreatedBy",
                value: "b8815d32-d907-4565-8799-49a5cebcdc4c");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Product");
        }
    }
}
