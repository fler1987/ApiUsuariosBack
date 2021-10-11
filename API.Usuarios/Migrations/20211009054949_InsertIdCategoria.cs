using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Usuarios.Migrations
{
    public partial class InsertIdCategoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPeriodo",
                table: "Configuracion",
                newName: "IdCategoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdCategoria",
                table: "Configuracion",
                newName: "IdPeriodo");
        }
    }
}
