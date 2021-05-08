using Microsoft.EntityFrameworkCore.Migrations;

namespace APIRest.Migrations
{
    public partial class CambiadoCampoIdPermiso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Permisos_PermisoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdPermiso",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "PermisoId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Permisos_PermisoId",
                table: "Usuarios",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Permisos_PermisoId",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "PermisoId",
                table: "Usuarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IdPermiso",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Permisos_PermisoId",
                table: "Usuarios",
                column: "PermisoId",
                principalTable: "Permisos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
