using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeShop.Migrations
{
    public partial class AnimePriceTypeFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Anime",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Anime",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
