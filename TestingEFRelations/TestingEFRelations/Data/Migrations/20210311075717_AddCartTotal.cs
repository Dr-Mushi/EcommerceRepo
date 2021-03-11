using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingEFRelations.Migrations
{
    public partial class AddCartTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmlImage_Product_ProductID",
                table: "SmlImage");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "SmlImage",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CartTotal",
                table: "Cart",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_SmlImage_Product_ProductID",
                table: "SmlImage",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmlImage_Product_ProductID",
                table: "SmlImage");

            migrationBuilder.DropColumn(
                name: "CartTotal",
                table: "Cart");

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "SmlImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SmlImage_Product_ProductID",
                table: "SmlImage",
                column: "ProductID",
                principalTable: "Product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
