using Microsoft.EntityFrameworkCore.Migrations;

namespace APIRest.Migrations
{
    public partial class AgregadoCampoDanioSiniestro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int?>(
        name: "DanioId",
        table: "Siniestros",
        nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Siniestros_DanioId",
                table: "Siniestros",
                column: "DanioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Siniestros_Danios_DanioId",
                table: "Siniestros",
                column: "DanioId",
                principalTable: "Danios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
