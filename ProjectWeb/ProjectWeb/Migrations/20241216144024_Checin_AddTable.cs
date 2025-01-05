using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWeb.Migrations
{
    /// <inheritdoc />
    public partial class Checin_AddTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckinID",
                table: "Checkin",
                newName: "IdCheckin");

            migrationBuilder.AlterColumn<decimal>(
                name: "GreutateBagaj",
                table: "Checkin",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<string>(
                name: "LocRezervat",
                table: "Checkin",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocRezervat",
                table: "Checkin");

            migrationBuilder.RenameColumn(
                name: "IdCheckin",
                table: "Checkin",
                newName: "CheckinID");

            migrationBuilder.AlterColumn<decimal>(
                name: "GreutateBagaj",
                table: "Checkin",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}
