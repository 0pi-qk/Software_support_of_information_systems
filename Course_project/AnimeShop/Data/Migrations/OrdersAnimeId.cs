using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimeShop.Migrations
{
    public partial class OrdersAnimeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Anime_AnimeId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PieId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "AnimeId",
                table: "OrderDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Anime_AnimeId",
                table: "OrderDetails",
                column: "AnimeId",
                principalTable: "Anime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Anime_AnimeId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "AnimeId",
                table: "OrderDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PieId",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Anime_AnimeId",
                table: "OrderDetails",
                column: "AnimeId",
                principalTable: "Anime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
