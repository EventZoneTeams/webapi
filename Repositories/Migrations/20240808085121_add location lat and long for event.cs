using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addlocationlatandlongforevent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "University",
                table: "Events",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "ReasonNote",
                table: "Events",
                newName: "LocationNote");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Events",
                newName: "LocationDisplay");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "EventImages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Events",
                newName: "University");

            migrationBuilder.RenameColumn(
                name: "LocationNote",
                table: "Events",
                newName: "ReasonNote");

            migrationBuilder.RenameColumn(
                name: "LocationDisplay",
                table: "Events",
                newName: "Location");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "EventImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
