using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddIdZborToUtilizator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdZbor",
                table: "Utilizator",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizator_IdZbor",
                table: "Utilizator",
                column: "IdZbor");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizator_Zbor_IdZbor",
                table: "Utilizator",
                column: "IdZbor",
                principalTable: "Zbor",
                principalColumn: "IdZbor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizator_Zbor_IdZbor",
                table: "Utilizator");

            migrationBuilder.DropIndex(
                name: "IX_Utilizator_IdZbor",
                table: "Utilizator");

            migrationBuilder.DropColumn(
                name: "IdZbor",
                table: "Utilizator");
        }
    }
}
