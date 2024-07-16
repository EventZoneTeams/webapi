using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class conflict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_EventProducts_EventProductId",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_EventProducts_EventProductId",
                table: "ProductImages",
                column: "EventProductId",
                principalTable: "EventProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_EventProducts_EventProductId",
                table: "ProductImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_EventProducts_EventProductId",
                table: "ProductImages",
                column: "EventProductId",
                principalTable: "EventProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
