using BibliotecaAPI.Entidades;

namespace BibliotecaAPI.Repositories;

public interface ILibroRepository
{
    List<Libro> ObtenerLibros();
    Libro? ObtenerLibro(int id);
    void AgregarLibro(Libro libro);
    void EliminarLibro(Libro libro);
    void GuardarCambios();
}
