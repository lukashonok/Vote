using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repositories.Migrations
{
    public partial class ChangeFileStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "CompromisingEvidenceFile");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CompromisingEvidenceFile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "CompromisingEvidenceFile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CompromisingEvidenceFile");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "CompromisingEvidenceFile");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "CompromisingEvidenceFile",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
