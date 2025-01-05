using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWeb.Migrations
{
    /// <inheritdoc />
    public partial class ZborClassUpdateStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Plecare",
                table: "Zbor",
                newName: "DataPlecare");

            migrationBuilder.RenameColumn(
                name: "NumeZbor",
                table: "Zbor",
                newName: "NumeCompanie");

            migrationBuilder.AlterColumn<decimal>(
                name: "GreutateMaximaBagaj",
                table: "Zbor",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<string>(
                name: "Imbarcare",
                table: "Zbor",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imbarcare",
                table: "Zbor");

            migrationBuilder.RenameColumn(
                name: "NumeCompanie",
                table: "Zbor",
                newName: "NumeZbor");

            migrationBuilder.RenameColumn(
                name: "DataPlecare",
                table: "Zbor",
                newName: "Plecare");

            migrationBuilder.AlterColumn<decimal>(
                name: "GreutateMaximaBagaj",
                table: "Zbor",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}
