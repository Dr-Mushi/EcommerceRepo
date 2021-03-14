using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingEFRelations.Migrations
{
    public partial class AddNewImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Image_ImageID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ImageID",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ProductsProductID",
                table: "Image",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ImageID",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ImageID",
                table: "Product",
                column: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Image_ImageID",
                table: "Product",
                column: "ImageID",
                principalTable: "Image",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
