using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserNameUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assignment_users_student_id",
                table: "assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_assignment_users_teacher_id",
                table: "assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_connection_request_users_student_id",
                table: "connection_request");

            migrationBuilder.DropForeignKey(
                name: "FK_email_verification_code_users_user_id",
                table: "email_verification_code");

            migrationBuilder.DropForeignKey(
                name: "FK_favorite_problem_users_student_id",
                table: "favorite_problem");

            migrationBuilder.DropForeignKey(
                name: "FK_favorite_test_users_student_id",
                table: "favorite_test");

            migrationBuilder.DropForeignKey(
                name: "FK_group_users_teacher_id",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_invitation_code_users_UserId",
                table: "invitation_code");

            migrationBuilder.DropForeignKey(
                name: "FK_invitation_code_users_teacher_id",
                table: "invitation_code");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_users_UserId",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_users_recipient_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_problem_users_author_id",
                table: "problem");

            migrationBuilder.DropForeignKey(
                name: "FK_student_group_student_users_student_id",
                table: "student_group_student");

            migrationBuilder.DropForeignKey(
                name: "FK_teacher_student_users_student_id",
                table: "teacher_student");

            migrationBuilder.DropForeignKey(
                name: "FK_teacher_student_users_teacher_id",
                table: "teacher_student");

            migrationBuilder.DropForeignKey(
                name: "FK_test_users_author_id",
                table: "test");

            migrationBuilder.DropForeignKey(
                name: "FK_test_attempt_users_student_id",
                table: "test_attempt");

            migrationBuilder.DropForeignKey(
                name: "FK_user_event_users_user_id",
                table: "user_event");

            migrationBuilder.DropForeignKey(
                name: "FK_user_session_users_user_id",
                table: "user_session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_username",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameIndex(
                name: "IX_users_email",
                table: "user",
                newName: "IX_user_email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_user_username",
                table: "user",
                column: "username");

            migrationBuilder.AddForeignKey(
                name: "FK_assignment_user_student_id",
                table: "assignment",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_assignment_user_teacher_id",
                table: "assignment",
                column: "teacher_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_connection_request_user_student_id",
                table: "connection_request",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_email_verification_code_user_user_id",
                table: "email_verification_code",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_problem_user_student_id",
                table: "favorite_problem",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_test_user_student_id",
                table: "favorite_test",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_user_teacher_id",
                table: "group",
                column: "teacher_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_invitation_code_user_UserId",
                table: "invitation_code",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_invitation_code_user_teacher_id",
                table: "invitation_code",
                column: "teacher_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_user_UserId",
                table: "notification",
                column: "UserId",
                principalTable: "user",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_user_recipient_id",
                table: "notification",
                column: "recipient_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_problem_user_author_id",
                table: "problem",
                column: "author_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_group_student_user_student_id",
                table: "student_group_student",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teacher_student_user_student_id",
                table: "teacher_student",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_teacher_student_user_teacher_id",
                table: "teacher_student",
                column: "teacher_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_test_user_author_id",
                table: "test",
                column: "author_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_test_attempt_user_student_id",
                table: "test_attempt",
                column: "student_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_event_user_user_id",
                table: "user_event",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_session_user_user_id",
                table: "user_session",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assignment_user_student_id",
                table: "assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_assignment_user_teacher_id",
                table: "assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_connection_request_user_student_id",
                table: "connection_request");

            migrationBuilder.DropForeignKey(
                name: "FK_email_verification_code_user_user_id",
                table: "email_verification_code");

            migrationBuilder.DropForeignKey(
                name: "FK_favorite_problem_user_student_id",
                table: "favorite_problem");

            migrationBuilder.DropForeignKey(
                name: "FK_favorite_test_user_student_id",
                table: "favorite_test");

            migrationBuilder.DropForeignKey(
                name: "FK_group_user_teacher_id",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_invitation_code_user_UserId",
                table: "invitation_code");

            migrationBuilder.DropForeignKey(
                name: "FK_invitation_code_user_teacher_id",
                table: "invitation_code");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_user_UserId",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_user_recipient_id",
                table: "notification");

            migrationBuilder.DropForeignKey(
                name: "FK_problem_user_author_id",
                table: "problem");

            migrationBuilder.DropForeignKey(
                name: "FK_student_group_student_user_student_id",
                table: "student_group_student");

            migrationBuilder.DropForeignKey(
                name: "FK_teacher_student_user_student_id",
                table: "teacher_student");

            migrationBuilder.DropForeignKey(
                name: "FK_teacher_student_user_teacher_id",
                table: "teacher_student");

            migrationBuilder.DropForeignKey(
                name: "FK_test_user_author_id",
                table: "test");

            migrationBuilder.DropForeignKey(
                name: "FK_test_attempt_user_student_id",
                table: "test_attempt");

            migrationBuilder.DropForeignKey(
                name: "FK_user_event_user_user_id",
                table: "user_event");

            migrationBuilder.DropForeignKey(
                name: "FK_user_session_user_user_id",
                table: "user_session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_username",
                table: "user");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_user_email",
                table: "users",
                newName: "IX_users_email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_assignment_users_student_id",
                table: "assignment",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_assignment_users_teacher_id",
                table: "assignment",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_connection_request_users_student_id",
                table: "connection_request",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_email_verification_code_users_user_id",
                table: "email_verification_code",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_problem_users_student_id",
                table: "favorite_problem",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_favorite_test_users_student_id",
                table: "favorite_test",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_users_teacher_id",
                table: "group",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_invitation_code_users_UserId",
                table: "invitation_code",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_invitation_code_users_teacher_id",
                table: "invitation_code",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_users_UserId",
                table: "notification",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_notification_users_recipient_id",
                table: "notification",
                column: "recipient_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_problem_users_author_id",
                table: "problem",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_student_group_student_users_student_id",
                table: "student_group_student",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teacher_student_users_student_id",
                table: "teacher_student",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_teacher_student_users_teacher_id",
                table: "teacher_student",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_test_users_author_id",
                table: "test",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_test_attempt_users_student_id",
                table: "test_attempt",
                column: "student_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_event_users_user_id",
                table: "user_event",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_session_users_user_id",
                table: "user_session",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
