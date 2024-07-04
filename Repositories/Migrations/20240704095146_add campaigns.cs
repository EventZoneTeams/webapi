using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class addcampaigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCampaign_Events_EventId",
                table: "EventCampaign");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCampaign",
                table: "EventCampaign");

            migrationBuilder.RenameTable(
                name: "EventCampaign",
                newName: "EventCampaigns");

            migrationBuilder.RenameIndex(
                name: "IX_EventCampaign_EventId",
                table: "EventCampaigns",
                newName: "IX_EventCampaigns_EventId");

            migrationBuilder.AlterColumn<long>(
                name: "TotalPrice",
                table: "EventPackages",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCampaigns",
                table: "EventCampaigns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCampaigns_Events_EventId",
                table: "EventCampaigns",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventCampaigns_Events_EventId",
                table: "EventCampaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventCampaigns",
                table: "EventCampaigns");

            migrationBuilder.RenameTable(
                name: "EventCampaigns",
                newName: "EventCampaign");

            migrationBuilder.RenameIndex(
                name: "IX_EventCampaigns_EventId",
                table: "EventCampaign",
                newName: "IX_EventCampaign_EventId");

            migrationBuilder.AlterColumn<int>(
                name: "TotalPrice",
                table: "EventPackages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventCampaign",
                table: "EventCampaign",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCampaign_Events_EventId",
                table: "EventCampaign",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
