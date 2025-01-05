using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWeb.Migrations
{
    /// <inheritdoc />
    public partial class Checin_UpdateTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkin_Utilizator_UtilizatorID",
                table: "Checkin");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkin_Zbor_ZborID",
                table: "Checkin");

            migrationBuilder.DropColumn(
                name: "EsteValid",
                table: "Checkin");

            migrationBuilder.RenameColumn(
                name: "ZborID",
                table: "Checkin",
                newName: "IdZbor");

            migrationBuilder.RenameColumn(
                name: "UtilizatorID",
                table: "Checkin",
                newName: "IdUtilizator");

            migrationBuilder.RenameIndex(
                name: "IX_Checkin_ZborID",
                table: "Checkin",
                newName: "IX_Checkin_IdZbor");

            migrationBuilder.RenameIndex(
                name: "IX_Checkin_UtilizatorID",
                table: "Checkin",
                newName: "IX_Checkin_IdUtilizator");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkin_Utilizator_IdUtilizator",
                table: "Checkin",
                column: "IdUtilizator",
                principalTable: "Utilizator",
                principalColumn: "IdUtilizator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkin_Zbor_IdZbor",
                table: "Checkin",
                column: "IdZbor",
                principalTable: "Zbor",
                principalColumn: "IdZbor",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkin_Utilizator_IdUtilizator",
                table: "Checkin");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkin_Zbor_IdZbor",
                table: "Checkin");

            migrationBuilder.RenameColumn(
                name: "IdZbor",
                table: "Checkin",
                newName: "ZborID");

            migrationBuilder.RenameColumn(
                name: "IdUtilizator",
                table: "Checkin",
                newName: "UtilizatorID");

            migrationBuilder.RenameIndex(
                name: "IX_Checkin_IdZbor",
                table: "Checkin",
                newName: "IX_Checkin_ZborID");

            migrationBuilder.RenameIndex(
                name: "IX_Checkin_IdUtilizator",
                table: "Checkin",
                newName: "IX_Checkin_UtilizatorID");

            migrationBuilder.AddColumn<bool>(
                name: "EsteValid",
                table: "Checkin",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkin_Utilizator_UtilizatorID",
                table: "Checkin",
                column: "UtilizatorID",
                principalTable: "Utilizator",
                principalColumn: "IdUtilizator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Checkin_Zbor_ZborID",
                table: "Checkin",
                column: "ZborID",
                principalTable: "Zbor",
                principalColumn: "IdZbor",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
