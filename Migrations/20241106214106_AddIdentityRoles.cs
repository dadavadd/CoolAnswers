using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoolAnswers.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "93143253-0c5e-49e7-99c0-23caf19ff978", null, "User", "USER" },
                    { "d9f50f84-ff60-41b7-808b-8540cc27f0e8", null, "Moderator", "MODERATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93143253-0c5e-49e7-99c0-23caf19ff978");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9f50f84-ff60-41b7-808b-8540cc27f0e8");
        }
    }
}
