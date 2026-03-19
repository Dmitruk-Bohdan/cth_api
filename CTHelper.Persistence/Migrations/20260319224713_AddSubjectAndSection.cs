using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectAndSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_topic_subject",
                table: "topic");

            migrationBuilder.DropIndex(
                name: "IX_test_subject",
                table: "test");

            migrationBuilder.DropIndex(
                name: "IX_group_subject",
                table: "group");

            migrationBuilder.DropColumn(
                name: "subject",
                table: "topic");

            migrationBuilder.DropColumn(
                name: "subject",
                table: "test");

            migrationBuilder.DropColumn(
                name: "subject",
                table: "group");

            migrationBuilder.AddColumn<long>(
                name: "section_id",
                table: "topic",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "subject_id",
                table: "test",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "subject_id",
                table: "group",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "subject",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "section",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject_id = table.Column<long>(type: "bigint", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()"),
                    last_update_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.id);
                    table.ForeignKey(
                        name: "FK_section_subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subject",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_topic_section_id",
                table: "topic",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_test_subject_id",
                table: "test",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_subject_id",
                table: "group",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_subject_id",
                table: "section",
                column: "subject_id");

            migrationBuilder.AddForeignKey(
                name: "FK_group_subject_subject_id",
                table: "group",
                column: "subject_id",
                principalTable: "subject",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_test_subject_subject_id",
                table: "test",
                column: "subject_id",
                principalTable: "subject",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_topic_section_section_id",
                table: "topic",
                column: "section_id",
                principalTable: "section",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_group_subject_subject_id",
                table: "group");

            migrationBuilder.DropForeignKey(
                name: "FK_test_subject_subject_id",
                table: "test");

            migrationBuilder.DropForeignKey(
                name: "FK_topic_section_section_id",
                table: "topic");

            migrationBuilder.DropTable(
                name: "section");

            migrationBuilder.DropTable(
                name: "subject");

            migrationBuilder.DropIndex(
                name: "IX_topic_section_id",
                table: "topic");

            migrationBuilder.DropIndex(
                name: "IX_test_subject_id",
                table: "test");

            migrationBuilder.DropIndex(
                name: "IX_group_subject_id",
                table: "group");

            migrationBuilder.DropColumn(
                name: "section_id",
                table: "topic");

            migrationBuilder.DropColumn(
                name: "subject_id",
                table: "test");

            migrationBuilder.DropColumn(
                name: "subject_id",
                table: "group");

            migrationBuilder.AddColumn<short>(
                name: "subject",
                table: "topic",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "subject",
                table: "test",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "subject",
                table: "group",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_topic_subject",
                table: "topic",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "IX_test_subject",
                table: "test",
                column: "subject");

            migrationBuilder.CreateIndex(
                name: "IX_group_subject",
                table: "group",
                column: "subject");
        }
    }
}
