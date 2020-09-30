using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class FixFilesRe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompromisingEvidence",
                table: "CompromisingEvidenceFile",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "CompromisingEvidenceFile",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompromisingEvidenceFile_CompromisingEvidence",
                table: "CompromisingEvidenceFile",
                column: "CompromisingEvidence");

            migrationBuilder.AddForeignKey(
                name: "FK_CompromisingEvidenceFile_CompromisingEvidence_CompromisingEvidence",
                table: "CompromisingEvidenceFile",
                column: "CompromisingEvidence",
                principalTable: "CompromisingEvidence",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompromisingEvidenceFile_CompromisingEvidence_CompromisingEvidence",
                table: "CompromisingEvidenceFile");

            migrationBuilder.DropIndex(
                name: "IX_CompromisingEvidenceFile_CompromisingEvidence",
                table: "CompromisingEvidenceFile");

            migrationBuilder.DropColumn(
                name: "CompromisingEvidence",
                table: "CompromisingEvidenceFile");

            migrationBuilder.DropColumn(
                name: "File",
                table: "CompromisingEvidenceFile");
        }
    }
}
