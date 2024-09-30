using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventZone.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class removePackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOrderDetails_EventPackages_EventPackageId",
                table: "EventOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_EventOrderDetails_EventPackageId",
                table: "EventOrderDetails");

            migrationBuilder.DropColumn(
                name: "EventPackageId",
                table: "EventOrderDetails");

            migrationBuilder.RenameColumn(
                name: "PackageId",
                table: "EventOrderDetails",
                newName: "EventProductId");

            migrationBuilder.CreateIndex(
                name: "IX_EventOrderDetails_EventProductId",
                table: "EventOrderDetails",
                column: "EventProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventOrderDetails_EventProducts_EventProductId",
                table: "EventOrderDetails",
                column: "EventProductId",
                principalTable: "EventProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventOrderDetails_EventProducts_EventProductId",
                table: "EventOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_EventOrderDetails_EventProductId",
                table: "EventOrderDetails");

            migrationBuilder.RenameColumn(
                name: "EventProductId",
                table: "EventOrderDetails",
                newName: "PackageId");

            migrationBuilder.AddColumn<Guid>(
                name: "EventPackageId",
                table: "EventOrderDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EventOrderDetails_EventPackageId",
                table: "EventOrderDetails",
                column: "EventPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventOrderDetails_EventPackages_EventPackageId",
                table: "EventOrderDetails",
                column: "EventPackageId",
                principalTable: "EventPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
