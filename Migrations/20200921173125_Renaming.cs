using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class Renaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetId",
                table: "Vote");

            migrationBuilder.AddColumn<int>(
                name: "Target",
                table: "Vote",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vote_Target",
                table: "Vote",
                column: "Target");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Target_Target",
                table: "Vote",
                column: "Target",
                principalTable: "Target",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Target_Target",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_Target",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "Target",
                table: "Vote");

            migrationBuilder.AddColumn<int>(
                name: "TargetId",
                table: "Vote",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
