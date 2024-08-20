using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class adjustmanytomanybetweeneventboardandeventboardlabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventBoards_EventBoardLabels_EventBoardLabelId",
                table: "EventBoards");

            migrationBuilder.DropIndex(
                name: "IX_EventBoards_EventBoardLabelId",
                table: "EventBoards");

            migrationBuilder.DropColumn(
                name: "EventBoardLabelId",
                table: "EventBoards");

            migrationBuilder.CreateTable(
                name: "EventBoardLabelAssignments",
                columns: table => new
                {
                    EventBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventBoardLabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardLabelAssignments", x => new { x.EventBoardId, x.EventBoardLabelId });
                    table.ForeignKey(
                        name: "FK_EventBoardLabelAssignments_EventBoardLabels_EventBoardLabelId",
                        column: x => x.EventBoardLabelId,
                        principalTable: "EventBoardLabels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventBoardLabelAssignments_EventBoards_EventBoardId",
                        column: x => x.EventBoardId,
                        principalTable: "EventBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardLabelAssignments_EventBoardLabelId",
                table: "EventBoardLabelAssignments",
                column: "EventBoardLabelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventBoardLabelAssignments");

            migrationBuilder.AddColumn<Guid>(
                name: "EventBoardLabelId",
                table: "EventBoards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EventBoards_EventBoardLabelId",
                table: "EventBoards",
                column: "EventBoardLabelId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventBoards_EventBoardLabels_EventBoardLabelId",
                table: "EventBoards",
                column: "EventBoardLabelId",
                principalTable: "EventBoardLabels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
