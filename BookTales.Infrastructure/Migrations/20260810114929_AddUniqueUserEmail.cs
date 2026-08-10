using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueUserEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DomainUsers_Email",
                table: "DomainUsers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DomainUsers_Email",
                table: "DomainUsers");
        }
    }
}
