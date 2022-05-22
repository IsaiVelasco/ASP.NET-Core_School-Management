using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class CreateTableCorteCaja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorteDeCaja",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CajaID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Calculado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Diferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    B1000 = table.Column<int>(type: "int", nullable: false),
                    B500 = table.Column<int>(type: "int", nullable: false),
                    B200 = table.Column<int>(type: "int", nullable: false),
                    B100 = table.Column<int>(type: "int", nullable: false),
                    B50 = table.Column<int>(type: "int", nullable: false),
                    B20 = table.Column<int>(type: "int", nullable: false),
                    M10 = table.Column<int>(type: "int", nullable: false),
                    M5 = table.Column<int>(type: "int", nullable: false),
                    M2 = table.Column<int>(type: "int", nullable: false),
                    M1 = table.Column<int>(type: "int", nullable: false),
                    C50 = table.Column<int>(type: "int", nullable: false),
                    C20 = table.Column<int>(type: "int", nullable: false),
                    C10 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorteDeCaja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CorteDeCaja_Caja_CajaID",
                        column: x => x.CajaID,
                        principalTable: "Caja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorteDeCaja_Persona_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Persona",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorteDeCaja_CajaID",
                table: "CorteDeCaja",
                column: "CajaID");

            migrationBuilder.CreateIndex(
                name: "IX_CorteDeCaja_UsuarioID",
                table: "CorteDeCaja",
                column: "UsuarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorteDeCaja");
        }
    }
}
