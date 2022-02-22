using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor.Server.Migrations
{
    public partial class seedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 7, "adidas LITE RACER 2.0 I su dječije patike za dječake. U njima će vaši mališani da nauče da balansiraju između snage i odgovornosti.", "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/EH1/EH1425/images/thumbs_800/thumbs_w/EH1425_800_800px_w.jpg", "adidas LITE RACER 2.0", 47.40m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 8, "Jaketa", "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/KRA/KRA203M504-01/images/thumbs_800/thumbs_w/KRA203M504-01_800_800px_w.jpg", "KRONOS Jakna Bee Jacket", 69.50m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Price" },
                values: new object[] { 9, "dukserica", "https://www.sportreality.ba/files/watermark/files/thumbs/files/images/slike_proizvoda/media/KRA/KRA221F602-80/images/thumbs_800/thumbs_w/KRA221F602-80_800_800px_w.jpg", "KRONOS LADIES CREWNECK", 52.20m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
