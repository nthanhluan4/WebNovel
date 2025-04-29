using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebNovel.Migrations
{
    /// <inheritdoc />
    public partial class addNewCol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChapterUpdatedAt",
                table: "Stories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChapterReadByDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChapterId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadCount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterReadByDates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoryVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StoryId = table.Column<int>(type: "int", nullable: false),
                    VoteCount = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryVotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserChapterReads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    StoryId = table.Column<int>(type: "int", nullable: false),
                    ChapterId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChapterReads", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterReadByDates");

            migrationBuilder.DropTable(
                name: "StoryVotes");

            migrationBuilder.DropTable(
                name: "UserChapterReads");

            migrationBuilder.DropColumn(
                name: "LastChapterUpdatedAt",
                table: "Stories");
        }
    }
}
