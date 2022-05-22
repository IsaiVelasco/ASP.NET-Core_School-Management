using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class AddFolioInscripcion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Folio",
                table: "Inscripcion",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Folio",
                table: "Inscripcion");
        }
    }
}
