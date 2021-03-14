using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingEFRelations.Migrations
{
    public partial class AddTotalQunatityReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiptProductQuantity",
                table: "Receipt",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ReceiptTotal",
                table: "Receipt",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptProductQuantity",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "ReceiptTotal",
                table: "Receipt");
        }
    }
}
