using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProvaTecnica.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a1e1b5f2-9f0a-4b0e-9f5a-1f1f1b0e9f5a", null, "Admin", "ADMIN" },
                    { "b2f2c6g3-0g1b-5c1f-0g6b-2g2g2c1f0g6b", null, "Professor", "PROFESSOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1e1b5f2-9f0a-4b0e-9f5a-1f1f1b0e9f5a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b2f2c6g3-0g1b-5c1f-0g6b-2g2g2c1f0g6b");
        }
    }
}
