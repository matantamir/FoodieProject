using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodieProject.Migrations
{
    public partial class AddPhoneToRestAndColorToTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tag",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Restaurant",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Restaurant");
        }
    }
}
