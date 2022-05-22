using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class HorarioCajaUsuarioCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilaHorario_Grado_GradoID",
                table: "FilaHorario");

            migrationBuilder.DropIndex(
                name: "IX_FilaHorario_GradoID",
                table: "FilaHorario");

            migrationBuilder.DropColumn(
                name: "GradoID",
                table: "FilaHorario");

            migrationBuilder.AddColumn<int>(
                name: "HorarioID",
                table: "FilaHorario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Horario",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupoID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horario", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Horario_Grupo_GrupoID",
                        column: x => x.GrupoID,
                        principalTable: "Grupo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilaHorario_HorarioID",
                table: "FilaHorario",
                column: "HorarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Horario_GrupoID",
                table: "Horario",
                column: "GrupoID");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaHorario_Horario_HorarioID",
                table: "FilaHorario",
                column: "HorarioID",
                principalTable: "Horario",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilaHorario_Horario_HorarioID",
                table: "FilaHorario");

            migrationBuilder.DropTable(
                name: "Horario");

            migrationBuilder.DropIndex(
                name: "IX_FilaHorario_HorarioID",
                table: "FilaHorario");

            migrationBuilder.DropColumn(
                name: "HorarioID",
                table: "FilaHorario");

            migrationBuilder.AddColumn<int>(
                name: "GradoID",
                table: "FilaHorario",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FilaHorario_GradoID",
                table: "FilaHorario",
                column: "GradoID");

            migrationBuilder.AddForeignKey(
                name: "FK_FilaHorario_Grado_GradoID",
                table: "FilaHorario",
                column: "GradoID",
                principalTable: "Grado",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
