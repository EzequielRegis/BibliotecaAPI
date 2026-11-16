using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly BibliotecaContext _context;

    public LibroRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public List<Libro> ObtenerLibros()
    {
        return _context.Libros
            .Include(l => l.Ejemplares)
            .ToList();
    }

    public Libro? ObtenerLibro(int id)
    {
        return _context.Libros
            .Include(l => l.Ejemplares)
                .ThenInclude(e => e.Prestamos)
            .FirstOrDefault(l => l.Id == id);
    }

    public void AgregarLibro(Libro libro)
    {
        _context.Libros.Add(libro);
    }

    public void EliminarLibro(Libro libro)
    {
        _context.Libros.Remove(libro);
    }

    public void GuardarCambios()
    {
        _context.SaveChanges();
    }
}
