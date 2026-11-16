using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProtegerHistorialPrestamos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_ejemplares_EjemplarId",
                table: "prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_lector_LectorId",
                table: "prestamos");

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_ejemplares_EjemplarId",
                table: "prestamos",
                column: "EjemplarId",
                principalTable: "ejemplares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_lector_LectorId",
                table: "prestamos",
                column: "LectorId",
                principalTable: "lector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_ejemplares_EjemplarId",
                table: "prestamos");

            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_lector_LectorId",
                table: "prestamos");

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_ejemplares_EjemplarId",
                table: "prestamos",
                column: "EjemplarId",
                principalTable: "ejemplares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_lector_LectorId",
                table: "prestamos",
                column: "LectorId",
                principalTable: "lector",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
