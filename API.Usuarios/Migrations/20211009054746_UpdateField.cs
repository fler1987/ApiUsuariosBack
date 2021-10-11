using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Usuarios.Migrations
{
    public partial class UpdateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Emplid",
                table: "Migracion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Migracion",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Periodo",
                table: "Migracion",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emplid",
                table: "Migracion");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Migracion");

            migrationBuilder.DropColumn(
                name: "Periodo",
                table: "Migracion");
        }
    }
}
