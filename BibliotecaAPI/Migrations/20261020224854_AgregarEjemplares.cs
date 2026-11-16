
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotecaAPI.Server.Migrations
{
    public partial class AgregarEjemplares : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ejemplares",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    LibroId = table.Column<int>(
                        type: "int",
                        nullable: false),

                    Disponible = table.Column<bool>(
                        type: "bit",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ejemplares", x => x.Id);

                    table.ForeignKey(
                        name: "FK_ejemplares_libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ejemplares_LibroId",
                table: "ejemplares",
                column: "LibroId");

            // Copiar cada libro existente a un ejemplar.
            // Se conserva el ID original para mantener la relación
            // entre los préstamos y los libros correspondientes.
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT ejemplares ON;

                INSERT INTO ejemplares (Id, LibroId, Disponible)
                SELECT
                    l.Id,
                    l.Id,
                    CASE
                        WHEN EXISTS (
                            SELECT 1
                            FROM prestamos p
                            WHERE p.LibroId = l.Id
                              AND p.EsDevuelto = 0
                        )
                        THEN 0
                        ELSE l.Disponible
                    END
                FROM libros l;

                SET IDENTITY_INSERT ejemplares OFF;
            ");

            migrationBuilder.DropForeignKey(
     name: "FK_Prestamos_Libros_LibroId",
     table: "prestamos");

            migrationBuilder.RenameColumn(
                name: "LibroId",
                table: "prestamos",
                newName: "EjemplarId");

            migrationBuilder.RenameIndex(
                name: "IX_prestamos_LibroId",
                table: "prestamos",
                newName: "IX_prestamos_EjemplarId");

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_ejemplares_EjemplarId",
                table: "prestamos",
                column: "EjemplarId",
                principalTable: "ejemplares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "Disponible",
                table: "libros");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Restaurar la disponibilidad del libro original.
            migrationBuilder.AddColumn<bool>(
                name: "Disponible",
                table: "libros",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.Sql(@"
                UPDATE l
                SET Disponible = CASE
                    WHEN EXISTS (
                        SELECT 1
                        FROM prestamos p
                        WHERE p.EjemplarId = l.Id
                          AND p.EsDevuelto = 0
                    )
                    THEN 0
                    ELSE 1
                END
                FROM libros l;
            ");

            migrationBuilder.DropForeignKey(
                name: "FK_prestamos_ejemplares_EjemplarId",
                table: "prestamos");

            migrationBuilder.RenameColumn(
                name: "EjemplarId",
                table: "prestamos",
                newName: "LibroId");

            migrationBuilder.RenameIndex(
                name: "IX_prestamos_EjemplarId",
                table: "prestamos",
                newName: "IX_prestamos_LibroId");

            migrationBuilder.AddForeignKey(
                name: "FK_prestamos_libros_LibroId",
                table: "prestamos",
                column: "LibroId",
                principalTable: "libros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropTable(
                name: "ejemplares");
        }
    }
}
