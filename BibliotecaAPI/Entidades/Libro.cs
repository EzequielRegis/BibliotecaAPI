
namespace BibliotecaAPI.Entidades;

public class Libro : EntidadBase
{
    public string Titulo { get; set; } = null!;
    public string Autor { get; set; } = "Autor desconocido";

    public ICollection<Ejemplar> Ejemplares { get; set; }
        = new List<Ejemplar>();

    
}
