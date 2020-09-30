using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class Add_CreatedAt_To_Evidence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CompromisingEvidence",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CompromisingEvidence");
        }
    }
}
