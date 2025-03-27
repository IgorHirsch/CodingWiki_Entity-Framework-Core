using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingWiki_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixBookAuthorMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthorMaps_Books_Book_Id",
                table: "BookAuthorMaps");

            migrationBuilder.RenameColumn(
                name: "Book_Id",
                table: "BookAuthorMaps",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthorMaps_Book_Id",
                table: "BookAuthorMaps",
                newName: "IX_BookAuthorMaps_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthorMaps_Books_BookId",
                table: "BookAuthorMaps",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthorMaps_Books_BookId",
                table: "BookAuthorMaps");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BookAuthorMaps",
                newName: "Book_Id");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthorMaps_BookId",
                table: "BookAuthorMaps",
                newName: "IX_BookAuthorMaps_Book_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthorMaps_Books_Book_Id",
                table: "BookAuthorMaps",
                column: "Book_Id",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
