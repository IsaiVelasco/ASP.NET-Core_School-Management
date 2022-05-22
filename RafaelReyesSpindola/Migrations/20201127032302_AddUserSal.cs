using Microsoft.EntityFrameworkCore.Migrations;

namespace RafaelReyesSpindola.Migrations
{
    public partial class AddUserSal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sal",
                table: "Persona",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sal",
                table: "Persona");
        }
    }
}
