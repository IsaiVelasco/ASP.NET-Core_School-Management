using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class UserCajaCorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Persona",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreUsuario",
                table: "Persona",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Persona",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Caja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MontoEfectivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Caja_Persona_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movimiento",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajaID = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoMovimiento = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimiento", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Movimiento_Caja_CajaID",
                        column: x => x.CajaID,
                        principalTable: "Caja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Caja_UsuarioID",
                table: "Caja",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_CajaID",
                table: "Movimiento",
                column: "CajaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movimiento");

            migrationBuilder.DropTable(
                name: "Caja");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "NombreUsuario",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Persona");
        }
    }
}
