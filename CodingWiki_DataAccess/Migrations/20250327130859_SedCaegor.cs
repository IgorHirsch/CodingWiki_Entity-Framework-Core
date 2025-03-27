using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodingWiki_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SedCaegor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                column: "Title",
                value: "Fortune of Time");


            migrationBuilder.Sql("INSERT INTO Categories VALUES ('Cat 1')");
            migrationBuilder.Sql("INSERT INTO Categories VALUES ('Cat 2')");
            migrationBuilder.Sql("INSERT INTO Categories VALUES ('Cat 3')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                column: "Title",
                value: "Fortune of time");
        }
    }
}
