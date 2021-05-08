using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIRest.Migrations
{
    public partial class CreadoClaseSiniestro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Siniestros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    AseguradoraId = table.Column<int>(type: "int", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioCreadoId = table.Column<int>(type: "int", nullable: false),
                    FechaHoraAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SujetoAfectado = table.Column<int>(type: "int", nullable: false),
                    ImpValoracionDanios = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    PeritoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Siniestros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Siniestros_Aseguradoras_AseguradoraId",
                        column: x => x.AseguradoraId,
                        principalTable: "Aseguradoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Siniestros_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Siniestros_Usuarios_PeritoId",
                        column: x => x.PeritoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Siniestros_Usuarios_UsuarioCreadoId",
                        column: x => x.UsuarioCreadoId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Siniestros_AseguradoraId",
                table: "Siniestros",
                column: "AseguradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_Siniestros_EstadoId",
                table: "Siniestros",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Siniestros_PeritoId",
                table: "Siniestros",
                column: "PeritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Siniestros_UsuarioCreadoId",
                table: "Siniestros",
                column: "UsuarioCreadoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Siniestros");
        }
    }
}
