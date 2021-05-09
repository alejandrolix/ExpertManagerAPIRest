using Microsoft.EntityFrameworkCore.Migrations;

namespace APIRest.Migrations
{
    public partial class CambiadoDecimalImpValoracionDanios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ImpValoracionDanios",
                table: "Siniestros",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");

            migrationBuilder.AddColumn<int>(
                name: "SiniestroId",
                table: "Danios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Danios_SiniestroId",
                table: "Danios",
                column: "SiniestroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Danios_Siniestros_SiniestroId",
                table: "Danios",
                column: "SiniestroId",
                principalTable: "Siniestros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Danios_Siniestros_SiniestroId",
                table: "Danios");

            migrationBuilder.DropIndex(
                name: "IX_Danios_SiniestroId",
                table: "Danios");

            migrationBuilder.DropColumn(
                name: "SiniestroId",
                table: "Danios");

            migrationBuilder.AlterColumn<decimal>(
                name: "ImpValoracionDanios",
                table: "Siniestros",
                type: "decimal(4,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}
