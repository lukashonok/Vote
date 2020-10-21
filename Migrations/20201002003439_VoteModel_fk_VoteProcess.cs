using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class VoteModel_fk_VoteProcess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Target_VoteProcess",
                table: "Vote");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_VoteProcess_VoteProcess",
                table: "Vote",
                column: "VoteProcess",
                principalTable: "VoteProcess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_VoteProcess_VoteProcess",
                table: "Vote");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Target_VoteProcess",
                table: "Vote",
                column: "VoteProcess",
                principalTable: "Target",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
