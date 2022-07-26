using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PalletWeight = table.Column<double>(type: "DOUBLE", nullable: false),
                    Height = table.Column<double>(type: "DOUBLE", nullable: false),
                    Width = table.Column<double>(type: "DOUBLE", nullable: false),
                    Length = table.Column<double>(type: "DOUBLE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductionDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Height = table.Column<double>(type: "DOUBLE", nullable: false),
                    Width = table.Column<double>(type: "DOUBLE", nullable: false),
                    Length = table.Column<double>(type: "DOUBLE", nullable: false),
                    Weight = table.Column<double>(type: "DOUBLE", nullable: false),
                    PalletId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boxes_Pallets_PalletId",
                        column: x => x.PalletId,
                        principalTable: "Pallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "Id",
                table: "Boxes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Boxes_PalletId",
                table: "Boxes",
                column: "PalletId");

            migrationBuilder.CreateIndex(
                name: "Id1",
                table: "Pallets",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boxes");

            migrationBuilder.DropTable(
                name: "Pallets");
        }
    }
}
