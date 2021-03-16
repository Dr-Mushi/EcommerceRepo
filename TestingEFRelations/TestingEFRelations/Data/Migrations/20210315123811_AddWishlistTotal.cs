using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingEFRelations.Migrations
{
    public partial class AddWishlistTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "WishlistTotal",
                table: "Wishlist",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WishlistTotal",
                table: "Wishlist");
        }
    }
}
