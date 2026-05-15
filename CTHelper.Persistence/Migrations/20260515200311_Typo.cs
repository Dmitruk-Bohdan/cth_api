using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Typo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notification_user_UserId",
                table: "notification");

            migrationBuilder.DropIndex(
                name: "IX_notification_UserId",
                table: "notification");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "notification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "notification",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_UserId",
                table: "notification",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_user_UserId",
                table: "notification",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id");
        }
    }
}
