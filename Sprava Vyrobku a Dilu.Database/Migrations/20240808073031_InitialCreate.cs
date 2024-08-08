using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpravaVyrobkuaDilu.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VyrobekModel",
                columns: table => new
                {
                    VyrobekId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Popis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cena = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    Poznamka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zalozeno = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Upraveno = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VyrobekModel", x => x.VyrobekId);
                });

            migrationBuilder.CreateTable(
                name: "DilModel",
                columns: table => new
                {
                    DilId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Popis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cena = table.Column<decimal>(type: "decimal(16,4)", precision: 16, scale: 4, nullable: false),
                    Zalozeno = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Upraveno = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VyrobekId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DilModel", x => x.DilId);
                    table.ForeignKey(
                        name: "FK_DilModel_VyrobekModel_VyrobekId",
                        column: x => x.VyrobekId,
                        principalTable: "VyrobekModel",
                        principalColumn: "VyrobekId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DilModel_VyrobekId",
                table: "DilModel",
                column: "VyrobekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DilModel");

            migrationBuilder.DropTable(
                name: "VyrobekModel");
        }
    }
}
