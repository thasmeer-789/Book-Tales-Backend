using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueItemConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId_BookId",
                table: "WishlistItems",
                columns: new[] { "WishlistId", "BookId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_BookId",
                table: "CartItems",
                columns: new[] { "CartId", "BookId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_WishlistId_BookId",
                table: "WishlistItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId_BookId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");
        }
    }
}
