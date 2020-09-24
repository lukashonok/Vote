using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class Add_X_Y_To_VotePlaceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "x",
                table: "VotePlace",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "y",
                table: "VotePlace",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "x",
                table: "VotePlace");

            migrationBuilder.DropColumn(
                name: "y",
                table: "VotePlace");
        }
    }
}
