using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wallet_tool.Migrations.WalletDb
{
    /// <inheritdoc />
    public partial class AddBalanceCheckandPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ToId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToId",
                table: "Transactions");
        }
    }
}
