using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet_tool.Migrations.WalletDb
{
    /// <inheritdoc />
    public partial class AddStatusandExpenditureFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Expenditure",
                table: "WalletAccount",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WalletAccount",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expenditure",
                table: "WalletAccount");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WalletAccount");
        }
    }
}
