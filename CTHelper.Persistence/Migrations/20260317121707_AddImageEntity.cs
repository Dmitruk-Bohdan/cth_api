using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "user");

            migrationBuilder.AddColumn<long>(
                name: "avatar_id",
                table: "user",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    object_key = table.Column<string>(type: "text", nullable: false),
                    owner_id = table.Column<long>(type: "bigint", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_image_user_owner_id",
                        column: x => x.owner_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_avatar_id",
                table: "user",
                column: "avatar_id");

            migrationBuilder.CreateIndex(
                name: "IX_image_object_key",
                table: "image",
                column: "object_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_image_owner_id",
                table: "image",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_image_avatar_id",
                table: "user",
                column: "avatar_id",
                principalTable: "image",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_image_avatar_id",
                table: "user");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropIndex(
                name: "IX_user_avatar_id",
                table: "user");

            migrationBuilder.DropColumn(
                name: "avatar_id",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "user",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);
        }
    }
}
