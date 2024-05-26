using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInPackages_EventPackages_EventPackageId",
                table: "ProductInPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInPackages_EventProducts_EventProductId",
                table: "ProductInPackages");

            migrationBuilder.DropIndex(
                name: "IX_ProductInPackages_EventPackageId",
                table: "ProductInPackages");

            migrationBuilder.DropIndex(
                name: "IX_ProductInPackages_EventProductId",
                table: "ProductInPackages");

            migrationBuilder.DropColumn(
                name: "EventPackageId",
                table: "ProductInPackages");

            migrationBuilder.DropColumn(
                name: "EventProductId",
                table: "ProductInPackages");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInPackages_PackageId",
                table: "ProductInPackages",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInPackages_EventPackages_PackageId",
                table: "ProductInPackages",
                column: "PackageId",
                principalTable: "EventPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInPackages_EventProducts_ProductId",
                table: "ProductInPackages",
                column: "ProductId",
                principalTable: "EventProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInPackages_EventPackages_PackageId",
                table: "ProductInPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInPackages_EventProducts_ProductId",
                table: "ProductInPackages");

            migrationBuilder.DropIndex(
                name: "IX_ProductInPackages_PackageId",
                table: "ProductInPackages");

            migrationBuilder.AddColumn<int>(
                name: "EventPackageId",
                table: "ProductInPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventProductId",
                table: "ProductInPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductInPackages_EventPackageId",
                table: "ProductInPackages",
                column: "EventPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInPackages_EventProductId",
                table: "ProductInPackages",
                column: "EventProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInPackages_EventPackages_EventPackageId",
                table: "ProductInPackages",
                column: "EventPackageId",
                principalTable: "EventPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInPackages_EventProducts_EventProductId",
                table: "ProductInPackages",
                column: "EventProductId",
                principalTable: "EventProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
