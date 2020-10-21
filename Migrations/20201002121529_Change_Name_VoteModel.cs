using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class Change_Name_VoteModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Vote");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberq",
                table: "Vote",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumberq",
                table: "Vote");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Vote",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
