using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingEFRelations.Migrations
{
    public partial class testSmallImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_SmlImage_ProductSmlImageID",
            //    table: "Product");

            //migrationBuilder.DropIndex(
            //    name: "IX_Product_ProductSmlImageID",
            //    table: "Product");

            //migrationBuilder.DropColumn(
            //    name: "ProductSmlImageID",
            //    table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "SmlImage",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmlImage_ProductID",
                table: "SmlImage",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_SmlImage_Product_ProductID",
                table: "SmlImage",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmlImage_Product_ProductID",
                table: "SmlImage");

            migrationBuilder.DropIndex(
                name: "IX_SmlImage_ProductID",
                table: "SmlImage");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "SmlImage");

            migrationBuilder.AddColumn<int>(
                name: "ProductSmlImageID",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductSmlImageID",
                table: "Product",
                column: "ProductSmlImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_SmlImage_ProductSmlImageID",
                table: "Product",
                column: "ProductSmlImageID",
                principalTable: "SmlImage",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
