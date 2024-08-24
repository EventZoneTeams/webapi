using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventZone.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addprioritycolumnineventtask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "EventBoardTasks",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "EventBoardTasks");
        }
    }
}
