using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlockedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "DomainUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "DomainUsers");
        }
    }
}
