using Microsoft.EntityFrameworkCore.Migrations;

namespace APIRest.Migrations
{
    public partial class AgregadoCampoEsPerito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsPerito",
                table: "Usuarios",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsPerito",
                table: "Usuarios");
        }
    }
}
