using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Server.Migrations
{
    /// <inheritdoc />
    public partial class QuitarRelacionBibliotecarioPrestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prestamos_Bibliotecarios_BibliotecarioId",
                table: "prestamos");

            migrationBuilder.DropIndex(
                name: "IX_Prestamos_BibliotecarioId",
                table: "prestamos");

            migrationBuilder.DropColumn(
                name: "BibliotecarioId",
                table: "prestamos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BibliotecarioId",
                table: "prestamos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_prestamos_BibliotecarioId",
                table: "prestamos",
                column: "BibliotecarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_bibliotecarios_BibliotecarioId",
                table: "prestamos",
                column: "BibliotecarioId",
                principalTable: "bibliotecarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
