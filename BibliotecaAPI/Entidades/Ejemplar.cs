
namespace BibliotecaAPI.Entidades;

public class Ejemplar : EntidadBase
{
    public int LibroId { get; set; }

    public Libro Libro { get; set; }= null!;

    public bool Disponible { get; set; } = true;

    public ICollection<Prestamo> Prestamos { get; set; }
        = new List<Prestamo>();
}
