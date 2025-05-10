using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "book",
                columns: table => new
                {
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_title = table.Column<string>(type: "text", nullable: false),
                    book_author = table.Column<string>(type: "text", nullable: false),
                    book_genre = table.Column<string>(type: "text", nullable: false),
                    book_quantity = table.Column<int>(type: "integer", nullable: false),
                    book_lent_quantity = table.Column<int>(type: "integer", nullable: false),
                    book_to_buy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book", x => x.book_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book");
        }
    }
}
