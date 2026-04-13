using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentDevision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assignment");

            migrationBuilder.CreateTable(
                name: "group_assignment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    default_attempts_allowed = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_assignment", x => x.id);
                    table.CheckConstraint("CK_assignment_positive_values", "default_attempts_allowed >= 0");
                    table.ForeignKey(
                        name: "FK_group_assignment_group_group_id",
                        column: x => x.group_id,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_group_assignment_test_test_id",
                        column: x => x.test_id,
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_group_assignment_user_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_assignment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    group_assignment_id = table.Column<long>(type: "bigint", nullable: false),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    attempts_left = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_assignment", x => x.id);
                    table.CheckConstraint("CK_assignment_positive_values", "attempts_left >= 0");
                    table.ForeignKey(
                        name: "FK_student_assignment_group_assignment_group_assignment_id",
                        column: x => x.group_assignment_id,
                        principalTable: "group_assignment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_assignment_test_test_id",
                        column: x => x.test_id,
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_student_assignment_user_student_id",
                        column: x => x.student_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_student_assignment_user_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_group_assignment_group_id",
                table: "group_assignment",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_assignment_teacher_id",
                table: "group_assignment",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_assignment_test_id",
                table: "group_assignment",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_group_assignment_id",
                table: "student_assignment",
                column: "group_assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_student_id",
                table: "student_assignment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_teacher_id",
                table: "student_assignment",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_assignment_test_id",
                table: "student_assignment",
                column: "test_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_assignment");

            migrationBuilder.DropTable(
                name: "group_assignment");

            migrationBuilder.CreateTable(
                name: "assignment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    student_id = table.Column<long>(type: "bigint", nullable: true),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    attempts_left = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignment", x => x.id);
                    table.CheckConstraint("CK_assignment_positive_values", "attempts_left >= 0");
                    table.CheckConstraint("CK_assignment_target", "(student_id IS NULL) <> (group_id IS NULL)");
                    table.ForeignKey(
                        name: "FK_assignment_group_group_id",
                        column: x => x.group_id,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_assignment_test_test_id",
                        column: x => x.test_id,
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assignment_user_student_id",
                        column: x => x.student_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_assignment_user_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assignment_group_id",
                table: "assignment",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_student_id",
                table: "assignment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_teacher_id",
                table: "assignment",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_assignment_test_id",
                table: "assignment",
                column: "test_id");
        }
    }
}
