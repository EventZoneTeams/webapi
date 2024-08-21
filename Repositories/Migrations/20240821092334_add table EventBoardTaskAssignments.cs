using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventZone.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addtableEventBoardTaskAssignments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventBoardTaskAssignment_AspNetUsers_UserId",
                table: "EventBoardTaskAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_EventBoardTaskAssignment_EventBoardTasks_EventBoardTaskId",
                table: "EventBoardTaskAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventBoardTaskAssignment",
                table: "EventBoardTaskAssignment");

            migrationBuilder.RenameTable(
                name: "EventBoardTaskAssignment",
                newName: "EventBoardTaskAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_EventBoardTaskAssignment_UserId",
                table: "EventBoardTaskAssignments",
                newName: "IX_EventBoardTaskAssignments_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventBoardTaskAssignments",
                table: "EventBoardTaskAssignments",
                columns: new[] { "EventBoardTaskId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventBoardTaskAssignments_AspNetUsers_UserId",
                table: "EventBoardTaskAssignments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventBoardTaskAssignments_EventBoardTasks_EventBoardTaskId",
                table: "EventBoardTaskAssignments",
                column: "EventBoardTaskId",
                principalTable: "EventBoardTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventBoardTaskAssignments_AspNetUsers_UserId",
                table: "EventBoardTaskAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_EventBoardTaskAssignments_EventBoardTasks_EventBoardTaskId",
                table: "EventBoardTaskAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventBoardTaskAssignments",
                table: "EventBoardTaskAssignments");

            migrationBuilder.RenameTable(
                name: "EventBoardTaskAssignments",
                newName: "EventBoardTaskAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_EventBoardTaskAssignments_UserId",
                table: "EventBoardTaskAssignment",
                newName: "IX_EventBoardTaskAssignment_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventBoardTaskAssignment",
                table: "EventBoardTaskAssignment",
                columns: new[] { "EventBoardTaskId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventBoardTaskAssignment_AspNetUsers_UserId",
                table: "EventBoardTaskAssignment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventBoardTaskAssignment_EventBoardTasks_EventBoardTaskId",
                table: "EventBoardTaskAssignment",
                column: "EventBoardTaskId",
                principalTable: "EventBoardTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
