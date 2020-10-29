using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class anothermigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "showResults",
                table: "VoteProcess",
                newName: "ShowResults");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowResults",
                table: "VoteProcess",
                newName: "showResults");
        }
    }
}
