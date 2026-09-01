namespace BibliotecaAPI.Entidades;

public class Libro
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public bool Disponible { get; set; }

    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}