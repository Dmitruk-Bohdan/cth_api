using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    entity_id = table.Column<long>(type: "bigint", nullable: false),
                    action = table.Column<short>(type: "smallint", nullable: false),
                    changes = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{}"),
                    timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_log", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "password_reset_token",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    token_hash = table.Column<string>(type: "char(128)", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    used_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_password_reset_token", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    session_id = table.Column<long>(type: "bigint", nullable: false),
                    token_hash = table.Column<string>(type: "char(128)", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    revoked_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "topic",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topic", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(48)", maxLength: 48, nullable: false),
                    password_hash = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: false),
                    role = table.Column<short>(type: "smallint", nullable: false),
                    last_login_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "app_group",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    subject = table.Column<short>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_app_group_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "email_verification_code",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    token_hash = table.Column<string>(type: "char(128)", nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    verified_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_verification_code", x => x.id);
                    table.ForeignKey(
                        name: "FK_email_verification_code_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invitation_code",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    uses_left = table.Column<short>(type: "smallint", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invitation_code", x => x.id);
                    table.CheckConstraint("CK_invitation_code_positive_values", "uses_left >= 0");
                    table.ForeignKey(
                        name: "FK_invitation_code_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_invitation_code_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recipient_id = table.Column<long>(type: "bigint", nullable: false),
                    priority_level = table.Column<short>(type: "smallint", nullable: false),
                    payload = table.Column<string>(type: "text", nullable: false),
                    is_seen = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notification_users_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "problem",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    topic_id = table.Column<long>(type: "bigint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_topic_topic_id",
                        column: x => x.topic_id,
                        principalTable: "topic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_problem_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teacher_student",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_student", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_student_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teacher_student_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "test",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject = table.Column<short>(type: "smallint", nullable: false),
                    author_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    is_traning = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    duration = table.Column<int>(type: "integer", nullable: true),
                    attempts_count = table.Column<short>(type: "smallint", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test", x => x.id);
                    table.CheckConstraint("CK_test_positive_values", "attempts_count >= 0 AND (duration IS NULL OR duration >= 0)");
                    table.ForeignKey(
                        name: "FK_test_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_event",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    event_type = table.Column<short>(type: "smallint", nullable: false),
                    target_id = table.Column<long>(type: "bigint", nullable: true),
                    ip_address = table.Column<NpgsqlInet>(type: "inet", nullable: true),
                    device_info = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_event", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_event_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_session",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    jti = table.Column<Guid>(type: "uuid", nullable: false),
                    client_type = table.Column<short>(type: "smallint", nullable: false),
                    ip_address = table.Column<NpgsqlInet>(type: "inet", nullable: true),
                    device_info = table.Column<string>(type: "jsonb", nullable: true),
                    last_activity_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    revoked_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_session", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_session_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_group_student",
                columns: table => new
                {
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_group_student", x => new { x.group_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_student_group_student_app_group_group_id",
                        column: x => x.group_id,
                        principalTable: "app_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_group_student_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "connection_request",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_connection_request", x => x.id);
                    table.ForeignKey(
                        name: "FK_connection_request_invitation_code_code_id",
                        column: x => x.code_id,
                        principalTable: "invitation_code",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_connection_request_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "favorite_problem",
                columns: table => new
                {
                    problem_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_problem", x => new { x.problem_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_favorite_problem_problem_problem_id",
                        column: x => x.problem_id,
                        principalTable: "problem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorite_problem_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "problem_version",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    problem_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    difficulty = table.Column<short>(type: "smallint", nullable: false),
                    statement = table.Column<string>(type: "jsonb", nullable: false),
                    correct_answer = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    explanation = table.Column<string>(type: "jsonb", nullable: false, defaultValue: "{}"),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_version", x => x.id);
                    table.ForeignKey(
                        name: "FK_problem_version_problem_problem_id",
                        column: x => x.problem_id,
                        principalTable: "problem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assignment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_id = table.Column<long>(type: "bigint", nullable: true),
                    teacher_id = table.Column<long>(type: "bigint", nullable: false),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    attempts_left = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignment", x => x.id);
                    table.CheckConstraint("CK_assignment_positive_values", "attempts_left >= 0");
                    table.CheckConstraint("CK_assignment_target", "(student_id IS NULL) <> (group_id IS NULL)");
                    table.ForeignKey(
                        name: "FK_assignment_app_group_group_id",
                        column: x => x.group_id,
                        principalTable: "app_group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_assignment_test_test_id",
                        column: x => x.test_id,
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assignment_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_assignment_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "favorite_test",
                columns: table => new
                {
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    test_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_test", x => new { x.student_id, x.test_id });
                    table.ForeignKey(
                        name: "FK_favorite_test_test_test_id",
                        column: x => x.test_id,
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorite_test_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_attempt",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    student_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    raw_score = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_attempt", x => x.id);
                    table.CheckConstraint("CK_test_attempt_positive_values", "(duration IS NULL OR duration >= 0) AND (raw_score IS NULL OR raw_score >= 0)");
                    table.ForeignKey(
                        name: "FK_test_attempt_test_test_id",
                        column: x => x.test_id,
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_test_attempt_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "test_problem",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    problem_id = table.Column<long>(type: "bigint", nullable: false),
                    code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_problem", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_problem_problem_problem_id",
                        column: x => x.problem_id,
                        principalTable: "problem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_test_problem_test_test_id",
                        column: x => x.test_id,
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_answer",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_attempt_id = table.Column<long>(type: "bigint", nullable: false),
                    problem_version_id = table.Column<long>(type: "bigint", nullable: false),
                    answer = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    is_correct = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_answer", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_answer_problem_version_problem_version_id",
                        column: x => x.problem_version_id,
                        principalTable: "problem_version",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_answer_test_attempt_test_attempt_id",
                        column: x => x.test_attempt_id,
                        principalTable: "test_attempt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_group_subject",
                table: "app_group",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "IX_app_group_teacher_id",
                table: "app_group",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_group_teacher_id_name",
                table: "app_group",
                columns: new[] { "teacher_id", "name" },
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_audit_log_entity_type_entity_id",
                table: "audit_log",
                columns: new[] { "entity_type", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_connection_request_code_id",
                table: "connection_request",
                column: "code_id");

            migrationBuilder.CreateIndex(
                name: "IX_connection_request_student_id",
                table: "connection_request",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_email_verification_code_expires_at",
                table: "email_verification_code",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_email_verification_code_token_hash",
                table: "email_verification_code",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_email_verification_code_user_id",
                table: "email_verification_code",
                column: "user_id",
                unique: true,
                filter: "verified_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_problem_student_id",
                table: "favorite_problem",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_test_student_id",
                table: "favorite_test",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_test_test_id",
                table: "favorite_test",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_invitation_code_code",
                table: "invitation_code",
                column: "code");

            migrationBuilder.CreateIndex(
                name: "IX_invitation_code_teacher_id",
                table: "invitation_code",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_invitation_code_UserId",
                table: "invitation_code",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_notification_recipient_id",
                table: "notification",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_recipient_id_is_seen",
                table: "notification",
                columns: new[] { "recipient_id", "is_seen" });

            migrationBuilder.CreateIndex(
                name: "IX_notification_UserId",
                table: "notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_token_expires_at",
                table: "password_reset_token",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_token_token_hash",
                table: "password_reset_token",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_password_reset_token_user_id",
                table: "password_reset_token",
                column: "user_id",
                unique: true,
                filter: "used_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_problem_author_id",
                table: "problem",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_problem_topic_id",
                table: "problem",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_problem_version_problem_id",
                table: "problem_version",
                column: "problem_id");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_expires_at",
                table: "refresh_token",
                column: "expires_at");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_session_id",
                table: "refresh_token",
                column: "session_id",
                unique: true,
                filter: "revoked_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_token_hash",
                table: "refresh_token",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_student_group_student_group_id",
                table: "student_group_student",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_group_student_student_id",
                table: "student_group_student",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_student_student_id",
                table: "teacher_student",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_student_teacher_id_student_id",
                table: "teacher_student",
                columns: new[] { "teacher_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_test_author_id",
                table: "test",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_subject",
                table: "test",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "IX_test_title",
                table: "test",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "IX_test_attempt_student_id",
                table: "test_attempt",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_attempt_test_id_student_id",
                table: "test_attempt",
                columns: new[] { "test_id", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_test_problem_problem_id",
                table: "test_problem",
                column: "problem_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_problem_test_id",
                table: "test_problem",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_problem_test_id_code",
                table: "test_problem",
                columns: new[] { "test_id", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_topic_subject",
                table: "topic",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "IX_user_answer_problem_version_id",
                table: "user_answer",
                column: "problem_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_answer_test_attempt_id",
                table: "user_answer",
                column: "test_attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_event_user_id",
                table: "user_event",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_session_jti",
                table: "user_session",
                column: "jti",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_session_user_id",
                table: "user_session",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assignment");

            migrationBuilder.DropTable(
                name: "audit_log");

            migrationBuilder.DropTable(
                name: "connection_request");

            migrationBuilder.DropTable(
                name: "email_verification_code");

            migrationBuilder.DropTable(
                name: "favorite_problem");

            migrationBuilder.DropTable(
                name: "favorite_test");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "password_reset_token");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "student_group_student");

            migrationBuilder.DropTable(
                name: "teacher_student");

            migrationBuilder.DropTable(
                name: "test_problem");

            migrationBuilder.DropTable(
                name: "user_answer");

            migrationBuilder.DropTable(
                name: "user_event");

            migrationBuilder.DropTable(
                name: "user_session");

            migrationBuilder.DropTable(
                name: "invitation_code");

            migrationBuilder.DropTable(
                name: "app_group");

            migrationBuilder.DropTable(
                name: "problem_version");

            migrationBuilder.DropTable(
                name: "test_attempt");

            migrationBuilder.DropTable(
                name: "problem");

            migrationBuilder.DropTable(
                name: "test");

            migrationBuilder.DropTable(
                name: "topic");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
