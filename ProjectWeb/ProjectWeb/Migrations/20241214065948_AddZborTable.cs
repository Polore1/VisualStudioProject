using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddZborTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Zbor",
                columns: table => new
                {
                    IdZbor = table.Column<int>(type: "int", nullable: false),
                    NameZbor = table.Column<string>(type: "longtext", nullable: true),
                    Destinatie = table.Column<string>(type: "longtext", nullable: true),
                    Plecare = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    GreutateMaximaBagaj = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LocuriDisponibile = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zbor", x => x.IdZbor);
                });

            migrationBuilder.CreateTable(
                name: "Checkin",
                columns: table => new
                {
                    CheckinID = table.Column<int>(type: "int", nullable: false),
                    UtilizatorID = table.Column<int>(type: "int", nullable: false),
                    ZborID = table.Column<int>(type: "int", nullable: false),
                    GreutateBagaj = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    EsteValid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataCheckin = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkin", x => x.CheckinID);
                    table.ForeignKey(
                        name: "FK_Checkin_Utilizator_UtilizatorID",
                        column: x => x.UtilizatorID,
                        principalTable: "Utilizator",
                        principalColumn: "IdUtilizator",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Checkin_Zbor_ZborID",
                        column: x => x.ZborID,
                        principalTable: "Zbor",
                        principalColumn: "IdZbor",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Checkin_UtilizatorID",
                table: "Checkin",
                column: "UtilizatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Checkin_ZborID",
                table: "Checkin",
                column: "ZborID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkin");

            migrationBuilder.DropTable(
                name: "Zbor");
        }
    }
}
