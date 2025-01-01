using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet_tool.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "123e4566-e89b-12d3-a456-426614174001",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "456e4566-e89b-12d3-a456-426614174566",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "manager", "MANAGER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "123e4566-e89b-12d3-a456-426614174001",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Reader", "READER" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "456e4566-e89b-12d3-a456-426614174566",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Writer", "WRITER" });
        }
    }
}
