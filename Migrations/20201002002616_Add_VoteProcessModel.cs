using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class Add_VoteProcessModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteProcess",
                table: "Vote",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VoteProcess",
                table: "Vote",
                column: "VoteProcess");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Target_VoteProcess",
                table: "Vote",
                column: "VoteProcess",
                principalTable: "Target",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Target_VoteProcess",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_VoteProcess",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "VoteProcess",
                table: "Vote");
        }
    }
}
