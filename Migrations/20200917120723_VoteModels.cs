using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Vote.Migrations
{
    public partial class VoteModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Target",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VotePlace",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Region = table.Column<string>(nullable: true),
                    Town = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    House = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotePlace", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompromisingEvidence",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotePlace = table.Column<int>(nullable: true),
                    ApplicationUser = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompromisingEvidence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompromisingEvidence_AspNetUsers_ApplicationUser",
                        column: x => x.ApplicationUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompromisingEvidence_VotePlace_VotePlace",
                        column: x => x.VotePlace,
                        principalTable: "VotePlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotePlace = table.Column<int>(nullable: true),
                    ApplicationUser = table.Column<string>(nullable: true),
                    TargetId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vote_AspNetUsers_ApplicationUser",
                        column: x => x.ApplicationUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vote_VotePlace_VotePlace",
                        column: x => x.VotePlace,
                        principalTable: "VotePlace",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompromisingEvidenceFile",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompromisingEvidence = table.Column<int>(nullable: true),
                    File = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompromisingEvidenceFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompromisingEvidenceFile_CompromisingEvidence_CompromisingEvidence",
                        column: x => x.CompromisingEvidence,
                        principalTable: "CompromisingEvidence",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompromisingEvidence_ApplicationUser",
                table: "CompromisingEvidence",
                column: "ApplicationUser");

            migrationBuilder.CreateIndex(
                name: "IX_CompromisingEvidence_VotePlace",
                table: "CompromisingEvidence",
                column: "VotePlace");

            migrationBuilder.CreateIndex(
                name: "IX_CompromisingEvidenceFile_CompromisingEvidence",
                table: "CompromisingEvidenceFile",
                column: "CompromisingEvidence");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_ApplicationUser",
                table: "Vote",
                column: "ApplicationUser");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VotePlace",
                table: "Vote",
                column: "VotePlace");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompromisingEvidenceFile");

            migrationBuilder.DropTable(
                name: "Target");

            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "CompromisingEvidence");

            migrationBuilder.DropTable(
                name: "VotePlace");
        }
    }
}
