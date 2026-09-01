using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBookDeleteRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CartItem -> Book
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Books_BookId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Books_BookId",
                table: "CartItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);


            // WishlistItem -> Book
            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Books_BookId",
                table: "WishlistItems");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Books_BookId",
                table: "WishlistItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);


            // OrderItem -> Book
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Books_BookId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Books_BookId",
                table: "CartItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Books_BookId",
                table: "WishlistItems");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Books_BookId",
                table: "WishlistItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
