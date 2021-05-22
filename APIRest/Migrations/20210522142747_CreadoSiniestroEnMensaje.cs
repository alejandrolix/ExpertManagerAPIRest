using Microsoft.EntityFrameworkCore.Migrations;

namespace APIRest.Migrations
{
    public partial class CreadoSiniestroEnMensaje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiniestroId",
                table: "Mensajes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_SiniestroId",
                table: "Mensajes",
                column: "SiniestroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensajes_Siniestros_SiniestroId",
                table: "Mensajes",
                column: "SiniestroId",
                principalTable: "Siniestros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensajes_Siniestros_SiniestroId",
                table: "Mensajes");

            migrationBuilder.DropIndex(
                name: "IX_Mensajes_SiniestroId",
                table: "Mensajes");

            migrationBuilder.DropColumn(
                name: "SiniestroId",
                table: "Mensajes");
        }
    }
}
