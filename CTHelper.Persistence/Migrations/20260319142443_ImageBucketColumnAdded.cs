using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CTHelper.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImageBucketColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "bucket",
                table: "image",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bucket",
                table: "image");
        }
    }
}
