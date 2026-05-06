using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TeacherStudentRemovingUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_teacher_student_teacher_id_student_id",
                table: "teacher_student");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_student_teacher_id",
                table: "teacher_student",
                column: "teacher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_teacher_student_teacher_id",
                table: "teacher_student");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_student_teacher_id_student_id",
                table: "teacher_student",
                columns: new[] { "teacher_id", "student_id" },
                unique: true);
        }
    }
}
