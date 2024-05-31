using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addfieldintoeventandfixtransactionvirtual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Transactions_TransactionId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionId",
                table: "Transactions",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Transactions_TransactionId",
                table: "Transactions",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
