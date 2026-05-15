using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TypoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "IsDeleted",
            //    table: "student_assignment",
            //    newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "attempt_left",
                table: "email_verification_code",
                newName: "attempts_left");

            //migrationBuilder.AlterColumn<bool>(
            //    name: "is_deleted",
            //    table: "student_assignment",
            //    type: "boolean",
            //    nullable: false,
            //    defaultValue: false,
            //    oldClrType: typeof(bool),
            //    oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "is_deleted",
            //    table: "student_assignment",
            //    newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "attempts_left",
                table: "email_verification_code",
                newName: "attempt_left");

            //migrationBuilder.AlterColumn<bool>(
            //    name: "IsDeleted",
            //    table: "student_assignment",
            //    type: "boolean",
            //    nullable: false,
            //    oldClrType: typeof(bool),
            //    oldType: "boolean",
            //    oldDefaultValue: false);
        }
    }
}
