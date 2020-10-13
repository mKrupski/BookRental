using Microsoft.EntityFrameworkCore.Migrations;

namespace BookRental.Infrastructure.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookStatus",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookStatus",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
