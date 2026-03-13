using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class someIndexesFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_user_session_session_id",
                table: "refresh_token",
                column: "session_id",
                principalTable: "user_session",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_user_session_session_id",
                table: "refresh_token");
        }
    }
}
