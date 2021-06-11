using Microsoft.EntityFrameworkCore.Migrations;

namespace FoodieProject.Migrations
{
    public partial class DeleteDishTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishDishTag");

            migrationBuilder.DropTable(
                name: "DishTag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DishTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DishDishTag",
                columns: table => new
                {
                    DishTagsId = table.Column<int>(type: "int", nullable: false),
                    DishesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishDishTag", x => new { x.DishTagsId, x.DishesId });
                    table.ForeignKey(
                        name: "FK_DishDishTag_Dish_DishesId",
                        column: x => x.DishesId,
                        principalTable: "Dish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DishDishTag_DishTag_DishTagsId",
                        column: x => x.DishTagsId,
                        principalTable: "DishTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishDishTag_DishesId",
                table: "DishDishTag",
                column: "DishesId");
        }
    }
}
