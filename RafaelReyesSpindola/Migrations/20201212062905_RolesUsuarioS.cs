using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class RolesUsuarioS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rol_Persona_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolesDeUsuarios",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesDeUsuarios", x => new { x.RolID, x.UsuarioID });
                    table.ForeignKey(
                        name: "FK_RolesDeUsuarios_Persona_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesDeUsuarios_Rol_RolID",
                        column: x => x.RolID,
                        principalTable: "Rol",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rol_UsuarioID",
                table: "Rol",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_RolesDeUsuarios_UsuarioID",
                table: "RolesDeUsuarios",
                column: "UsuarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolesDeUsuarios");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
