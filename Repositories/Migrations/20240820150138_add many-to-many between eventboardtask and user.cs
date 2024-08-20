using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addmanytomanybetweeneventboardtaskanduser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EventBoards_EventBoardId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_EventBoardTasks_AspNetUsers_AssignedTo",
                table: "EventBoardTasks");

            migrationBuilder.DropIndex(
                name: "IX_EventBoardTasks_AssignedTo",
                table: "EventBoardTasks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EventBoardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "EventBoardTasks");

            migrationBuilder.DropColumn(
                name: "EventBoardId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "EventBoards",
                newName: "ImageUrl");

            migrationBuilder.CreateTable(
                name: "EventBoardMembers",
                columns: table => new
                {
                    EventBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardMembers", x => new { x.EventBoardId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventBoardMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventBoardMembers_EventBoards_EventBoardId",
                        column: x => x.EventBoardId,
                        principalTable: "EventBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventBoardTaskAssignment",
                columns: table => new
                {
                    EventBoardTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardTaskAssignment", x => new { x.EventBoardTaskId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EventBoardTaskAssignment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventBoardTaskAssignment_EventBoardTasks_EventBoardTaskId",
                        column: x => x.EventBoardTaskId,
                        principalTable: "EventBoardTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardMembers_UserId",
                table: "EventBoardMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardTaskAssignment_UserId",
                table: "EventBoardTaskAssignment",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventBoardMembers");

            migrationBuilder.DropTable(
                name: "EventBoardTaskAssignment");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "EventBoards",
                newName: "Image");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignedTo",
                table: "EventBoardTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventBoardId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardTasks_AssignedTo",
                table: "EventBoardTasks",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EventBoardId",
                table: "AspNetUsers",
                column: "EventBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EventBoards_EventBoardId",
                table: "AspNetUsers",
                column: "EventBoardId",
                principalTable: "EventBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventBoardTasks_AspNetUsers_AssignedTo",
                table: "EventBoardTasks",
                column: "AssignedTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
