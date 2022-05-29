using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class AddPrintersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rol_Persona_UsuarioID",
                table: "Rol");

            migrationBuilder.DropIndex(
                name: "IX_Rol_UsuarioID",
                table: "Rol");

            migrationBuilder.DropColumn(
                name: "UsuarioID",
                table: "Rol");

            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Printers");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioID",
                table: "Rol",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rol_UsuarioID",
                table: "Rol",
                column: "UsuarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rol_Persona_UsuarioID",
                table: "Rol",
                column: "UsuarioID",
                principalTable: "Persona",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
