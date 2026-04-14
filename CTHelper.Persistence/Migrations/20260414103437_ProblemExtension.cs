using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProblemExtension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_assignment_user_student_id",
                table: "student_assignment");

            migrationBuilder.AlterColumn<long>(
                name: "group_assignment_id",
                table: "student_assignment",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "is_public",
                table: "problem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_published",
                table: "problem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_student_assignment_user_student_id",
                table: "student_assignment",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_student_assignment_user_student_id",
                table: "student_assignment");

            migrationBuilder.DropColumn(
                name: "is_public",
                table: "problem");

            migrationBuilder.DropColumn(
                name: "is_published",
                table: "problem");

            migrationBuilder.AlterColumn<long>(
                name: "group_assignment_id",
                table: "student_assignment",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_student_assignment_user_student_id",
                table: "student_assignment",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
