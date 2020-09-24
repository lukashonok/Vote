using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class Add_PhoneNumber_To_VoteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Vote",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Vote");
        }
    }
}
