using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addeventboard6table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderType",
                table: "EventOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "EventBoardId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventBoardLabels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventBoardLabels_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventBoards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventBoardLabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventBoards_AspNetUsers_LeaderId",
                        column: x => x.LeaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventBoards_EventBoardLabels_EventBoardLabelId",
                        column: x => x.EventBoardLabelId,
                        principalTable: "EventBoardLabels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventBoards_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventBoardColumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardColumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventBoardColumns_EventBoards_EventBoardId",
                        column: x => x.EventBoardId,
                        principalTable: "EventBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventBoardTaskLabels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardTaskLabels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventBoardTaskLabels_EventBoards_EventBoardId",
                        column: x => x.EventBoardId,
                        principalTable: "EventBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventBoardTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventBoardColumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventBoardTasks_AspNetUsers_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventBoardTasks_EventBoardColumns_EventBoardColumnId",
                        column: x => x.EventBoardColumnId,
                        principalTable: "EventBoardColumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventBoardTasks_EventBoards_EventBoardId",
                        column: x => x.EventBoardId,
                        principalTable: "EventBoards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventBoardTaskLabelAssignments",
                columns: table => new
                {
                    EventBoardTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventBoardTaskLabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventBoardTaskLabelAssignments", x => new { x.EventBoardTaskId, x.EventBoardTaskLabelId });
                    table.ForeignKey(
                        name: "FK_EventBoardTaskLabelAssignments_EventBoardTaskLabels_EventBoardTaskLabelId",
                        column: x => x.EventBoardTaskLabelId,
                        principalTable: "EventBoardTaskLabels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventBoardTaskLabelAssignments_EventBoardTasks_EventBoardTaskId",
                        column: x => x.EventBoardTaskId,
                        principalTable: "EventBoardTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EventBoardId",
                table: "AspNetUsers",
                column: "EventBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardColumns_EventBoardId",
                table: "EventBoardColumns",
                column: "EventBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardLabels_EventId",
                table: "EventBoardLabels",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoards_EventBoardLabelId",
                table: "EventBoards",
                column: "EventBoardLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoards_EventId",
                table: "EventBoards",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoards_LeaderId",
                table: "EventBoards",
                column: "LeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardTaskLabelAssignments_EventBoardTaskLabelId",
                table: "EventBoardTaskLabelAssignments",
                column: "EventBoardTaskLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardTaskLabels_EventBoardId",
                table: "EventBoardTaskLabels",
                column: "EventBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardTasks_AssignedTo",
                table: "EventBoardTasks",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardTasks_EventBoardColumnId",
                table: "EventBoardTasks",
                column: "EventBoardColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_EventBoardTasks_EventBoardId",
                table: "EventBoardTasks",
                column: "EventBoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EventBoards_EventBoardId",
                table: "AspNetUsers",
                column: "EventBoardId",
                principalTable: "EventBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EventBoards_EventBoardId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EventBoardTaskLabelAssignments");

            migrationBuilder.DropTable(
                name: "EventBoardTaskLabels");

            migrationBuilder.DropTable(
                name: "EventBoardTasks");

            migrationBuilder.DropTable(
                name: "EventBoardColumns");

            migrationBuilder.DropTable(
                name: "EventBoards");

            migrationBuilder.DropTable(
                name: "EventBoardLabels");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EventBoardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "EventOrders");

            migrationBuilder.DropColumn(
                name: "EventBoardId",
                table: "AspNetUsers");
        }
    }
}
