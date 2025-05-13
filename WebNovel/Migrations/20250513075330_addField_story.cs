using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNovel.Migrations
{
    /// <inheritdoc />
    public partial class addField_story : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Stories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Stories");
        }
    }
}
