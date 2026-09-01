namespace BibliotecaAPI.Entidades;

public class Prestamo
{
    public int Id { get; set; }

    public int LibroId { get; set; }
    public Libro Libro { get; set; }

    public int LectorId { get; set; }
    public Lector Lector { get; set; }

    public int BibliotecarioId { get; set; }
    public Bibliotecario Bibliotecario { get; set; }

    public DateTime FechaPrestamo { get; set; }
}
