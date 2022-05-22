using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class HorarioAddGrupoID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horario_Grupo_GrupoID",
                table: "Horario");

            migrationBuilder.AlterColumn<int>(
                name: "GrupoID",
                table: "Horario",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Horario_Grupo_GrupoID",
                table: "Horario",
                column: "GrupoID",
                principalTable: "Grupo",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Horario_Grupo_GrupoID",
                table: "Horario");

            migrationBuilder.AlterColumn<int>(
                name: "GrupoID",
                table: "Horario",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Horario_Grupo_GrupoID",
                table: "Horario",
                column: "GrupoID",
                principalTable: "Grupo",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
