using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet_tool.Migrations.WalletDb
{
    /// <inheritdoc />
    public partial class ChangeTransactionsFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Transactions",
                newName: "Transaction_Type");

            migrationBuilder.RenameColumn(
                name: "AccountName",
                table: "Transactions",
                newName: "To");

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Transaction_Type",
                table: "Transactions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "Transactions",
                newName: "AccountName");
        }
    }
}
