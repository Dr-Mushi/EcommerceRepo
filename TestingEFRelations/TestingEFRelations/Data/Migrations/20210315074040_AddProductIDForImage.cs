using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingEFRelations.Migrations
{
    public partial class AddProductIDForImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Product_ProductsProductID",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_ProductsProductID",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "ProductsProductID",
                table: "Image");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Image",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Image_ProductID",
                table: "Image",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Product_ProductID",
                table: "Image",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Product_ProductID",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_ProductID",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Image");

            migrationBuilder.AddColumn<int>(
                name: "ProductsProductID",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_ProductsProductID",
                table: "Image",
                column: "ProductsProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Product_ProductsProductID",
                table: "Image",
                column: "ProductsProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
