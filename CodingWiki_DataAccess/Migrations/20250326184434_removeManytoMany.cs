using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingWiki_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeManytoMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_Books_Fluent_Publishers_Publisher_Id",
                table: "Fluent_Books");

            migrationBuilder.DropTable(
                name: "Fluent_AuthorFluent_Book");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_Books_Publisher_Id",
                table: "Fluent_Books");

            migrationBuilder.AddColumn<int>(
                name: "Publisher_Id1",
                table: "Fluent_Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_Books_Publisher_Id1",
                table: "Fluent_Books",
                column: "Publisher_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_Books_Fluent_Publishers_Publisher_Id1",
                table: "Fluent_Books",
                column: "Publisher_Id1",
                principalTable: "Fluent_Publishers",
                principalColumn: "Publisher_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_Books_Fluent_Publishers_Publisher_Id1",
                table: "Fluent_Books");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_Books_Publisher_Id1",
                table: "Fluent_Books");

            migrationBuilder.DropColumn(
                name: "Publisher_Id1",
                table: "Fluent_Books");

            migrationBuilder.CreateTable(
                name: "Fluent_AuthorFluent_Book",
                columns: table => new
                {
                    AuthorsAuthor_Id = table.Column<int>(type: "int", nullable: false),
                    BooksBookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fluent_AuthorFluent_Book", x => new { x.AuthorsAuthor_Id, x.BooksBookId });
                    table.ForeignKey(
                        name: "FK_Fluent_AuthorFluent_Book_Fluent_Authors_AuthorsAuthor_Id",
                        column: x => x.AuthorsAuthor_Id,
                        principalTable: "Fluent_Authors",
                        principalColumn: "Author_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fluent_AuthorFluent_Book_Fluent_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Fluent_Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_Books_Publisher_Id",
                table: "Fluent_Books",
                column: "Publisher_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_AuthorFluent_Book_BooksBookId",
                table: "Fluent_AuthorFluent_Book",
                column: "BooksBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_Books_Fluent_Publishers_Publisher_Id",
                table: "Fluent_Books",
                column: "Publisher_Id",
                principalTable: "Fluent_Publishers",
                principalColumn: "Publisher_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
