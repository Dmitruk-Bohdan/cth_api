using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GroupTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_app_group_users_teacher_id",
                table: "app_group");

            migrationBuilder.DropForeignKey(
                name: "FK_assignment_app_group_group_id",
                table: "assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_student_group_student_app_group_group_id",
                table: "student_group_student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_app_group",
                table: "app_group");

            migrationBuilder.RenameTable(
                name: "app_group",
                newName: "group");

            migrationBuilder.RenameIndex(
                name: "IX_app_group_teacher_id_name",
                table: "group",
                newName: "IX_group_teacher_id_name");

            migrationBuilder.RenameIndex(
                name: "IX_app_group_teacher_id",
                table: "group",
                newName: "IX_group_teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_app_group_subject",
                table: "group",
                newName: "IX_group_subject");

            migrationBuilder.AddPrimaryKey(
                name: "PK_group",
                table: "group",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_assignment_group_group_id",
                table: "assignment",
                column: "group_id",
                principalTable: "group",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_group_users_teacher_id",
                table: "group",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_group_student_group_group_id",
                table: "student_group_student",
                column: "group_id",
                principalTable: "group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assignment_group_group_id",
                table: "assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_group_users_teacher_id",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_student_group_student_group_group_id",
                table: "student_group_student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_group",
                table: "group");

            migrationBuilder.RenameTable(
                name: "group",
                newName: "app_group");

            migrationBuilder.RenameIndex(
                name: "IX_group_teacher_id_name",
                table: "app_group",
                newName: "IX_app_group_teacher_id_name");

            migrationBuilder.RenameIndex(
                name: "IX_group_teacher_id",
                table: "app_group",
                newName: "IX_app_group_teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_group_subject",
                table: "app_group",
                newName: "IX_app_group_subject");

            migrationBuilder.AddPrimaryKey(
                name: "PK_app_group",
                table: "app_group",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_app_group_users_teacher_id",
                table: "app_group",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_assignment_app_group_group_id",
                table: "assignment",
                column: "group_id",
                principalTable: "app_group",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_student_group_student_app_group_group_id",
                table: "student_group_student",
                column: "group_id",
                principalTable: "app_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
