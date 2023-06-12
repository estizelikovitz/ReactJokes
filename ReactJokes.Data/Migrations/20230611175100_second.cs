using Microsoft.EntityFrameworkCore.Migrations;

namespace ReactJokes.Data.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Jokes");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Jokes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Jokes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Jokes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
