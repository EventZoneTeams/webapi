using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventZone.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class isReceived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReceived",
                table: "EventOrderDetails",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReceived",
                table: "EventOrderDetails");
        }
    }
}
