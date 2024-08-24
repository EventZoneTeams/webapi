using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventZone.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class imagefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "AspNetUsers",
                newName: "ImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "EventTickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "EventTickets");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "AspNetUsers",
                newName: "Image");
        }
    }
}
