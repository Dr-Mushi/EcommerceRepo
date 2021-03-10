using Microsoft.EntityFrameworkCore.Migrations;

namespace TestingEFRelations.Migrations
{
    public partial class parentChildDesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmlImage_Image_ImageID",
                table: "SmlImage");

            migrationBuilder.DropIndex(
                name: "IX_SmlImage_ImageID",
                table: "SmlImage");

            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "SmlImage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageID",
                table: "SmlImage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SmlImage_ImageID",
                table: "SmlImage",
                column: "ImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_SmlImage_Image_ImageID",
                table: "SmlImage",
                column: "ImageID",
                principalTable: "Image",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
