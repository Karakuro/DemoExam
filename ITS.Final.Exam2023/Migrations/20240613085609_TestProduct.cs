using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITS.Final.Exam2023.Migrations
{
    /// <inheritdoc />
    public partial class TestProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Description" },
                values: new object[] { "TestProduct", "TestDescription" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: "TestProduct");
        }
    }
}
