using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CodingWiki_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BokDeai : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookDetails",
                columns: new[] { "BookDetail_Id", "Book_Id", "NumberOfChapters", "NumberOfPages", "Weight" },
                values: new object[,]
                {
                    { 1, 1, 12, 350, "1.2kg" },
                    { 2, 2, 20, 500, "1.8kg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookDetails",
                keyColumn: "BookDetail_Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookDetails",
                keyColumn: "BookDetail_Id",
                keyValue: 2);
        }
    }
}
