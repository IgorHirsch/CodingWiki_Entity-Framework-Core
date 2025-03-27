using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingWiki_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FluentBookAuthorMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthorMap_Authors_Author_Id",
                table: "BookAuthorMap");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthorMap_Books_Book_Id",
                table: "BookAuthorMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookAuthorMap",
                table: "BookAuthorMap");

            migrationBuilder.RenameTable(
                name: "BookAuthorMap",
                newName: "BookAuthorMaps");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthorMap_Book_Id",
                table: "BookAuthorMaps",
                newName: "IX_BookAuthorMaps_Book_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookAuthorMaps",
                table: "BookAuthorMaps",
                columns: new[] { "Author_Id", "Book_Id" });

            migrationBuilder.CreateTable(
                name: "Fluent_BookAuthorMaps",
                columns: table => new
                {
                    Book_Id = table.Column<int>(type: "int", nullable: false),
                    Author_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fluent_BookAuthorMaps", x => new { x.Author_Id, x.Book_Id });
                    table.ForeignKey(
                        name: "FK_Fluent_BookAuthorMaps_Fluent_Authors_Author_Id",
                        column: x => x.Author_Id,
                        principalTable: "Fluent_Authors",
                        principalColumn: "Author_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fluent_BookAuthorMaps_Fluent_Books_Book_Id",
                        column: x => x.Book_Id,
                        principalTable: "Fluent_Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookAuthorMaps_Book_Id",
                table: "Fluent_BookAuthorMaps",
                column: "Book_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthorMaps_Authors_Author_Id",
                table: "BookAuthorMaps",
                column: "Author_Id",
                principalTable: "Authors",
                principalColumn: "Author_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthorMaps_Books_Book_Id",
                table: "BookAuthorMaps",
                column: "Book_Id",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthorMaps_Authors_Author_Id",
                table: "BookAuthorMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthorMaps_Books_Book_Id",
                table: "BookAuthorMaps");

            migrationBuilder.DropTable(
                name: "Fluent_BookAuthorMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookAuthorMaps",
                table: "BookAuthorMaps");

            migrationBuilder.RenameTable(
                name: "BookAuthorMaps",
                newName: "BookAuthorMap");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthorMaps_Book_Id",
                table: "BookAuthorMap",
                newName: "IX_BookAuthorMap_Book_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookAuthorMap",
                table: "BookAuthorMap",
                columns: new[] { "Author_Id", "Book_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthorMap_Authors_Author_Id",
                table: "BookAuthorMap",
                column: "Author_Id",
                principalTable: "Authors",
                principalColumn: "Author_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthorMap_Books_Book_Id",
                table: "BookAuthorMap",
                column: "Book_Id",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
