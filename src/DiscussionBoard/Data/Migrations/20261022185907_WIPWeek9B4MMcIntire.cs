using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscussionBoard.Data.Migrations
{
    /// <inheritdoc />
    public partial class WIPWeek9B4MMcIntire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionThreads_AspNetUsers_UserId",
                table: "DiscussionThreads");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionThreads_UserId",
                table: "DiscussionThreads");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DiscussionThreads");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "DiscussionThreads",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionThreads_UserId",
                table: "DiscussionThreads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionThreads_AspNetUsers_UserId",
                table: "DiscussionThreads",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
