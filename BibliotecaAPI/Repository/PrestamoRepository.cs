using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Repositories;

public class PrestamoRepository : IPrestamoRepository
{
    private readonly BibliotecaContext _context;

    public PrestamoRepository(BibliotecaContext context)
    {
        _context = context;
    }
    public static decimal CalcularMulta(double horasRetraso)
    {
        if (horasRetraso <= 0)
        {
            return 0;
        }

        int periodosCompletos = (int)(horasRetraso / 24);
        return periodosCompletos * 5000m;
    }
    public Libro? ObtenerLibro(int id)
    {
        return _context.Libros
            .Include(l => l.Ejemplares)
            .FirstOrDefault(l => l.Id == id);
    }

    public Lector? ObtenerLector(int id)
    {
        return _context.Lectores.Find(id);
    }


    public List<Ejemplar> ObtenerEjemplaresDisponibles(int libroId)
    {
        return _context.Ejemplares
            .Where(e => e.LibroId == libroId && e.Disponible)
            .ToList();
    }

    public Ejemplar? ObtenerEjemplarDisponible(int libroId)
    {
        return _context.Ejemplares
            .FirstOrDefault(e => e.LibroId == libroId && e.Disponible);
    }


    public Prestamo? ObtenerPrestamo(int id)
    {
        return _context.Prestamos
            .Include(p => p.Ejemplar)
                .ThenInclude(e => e.Libro)
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Prestamo> ObtenerPrestamos()
    {
        return _context.Prestamos
            .Include(p => p.Ejemplar)
                .ThenInclude(e => e.Libro)
            .Include(p => p.Lector)
            .ToList();
    }

    public void AgregarPrestamo(Prestamo prestamo)
    {
        _context.Prestamos.Add(prestamo);
    }

    public void ActualizarDisponibilidad(Ejemplar ejemplar)
    {
        ejemplar.Disponible = false;
    }

    public void GuardarCambios()
    {
        _context.SaveChanges();
    }


    public void RegistrarDevolucion(
        Prestamo prestamo,
        DateTime fechaDevolucionReal)
    {
        if (prestamo.FechaDevolucionPrevista == default)
        {
            throw new InvalidOperationException(
                "El préstamo no tiene una fecha de devolución prevista válida.");
        }

        prestamo.FechaDevolucionReal = fechaDevolucionReal;
        prestamo.EsDevuelto = true;

        double horasRetraso = (
             fechaDevolucionReal -
             prestamo.FechaDevolucionPrevista
                ).TotalHours;

        prestamo.Multa = CalcularMulta(horasRetraso);

        prestamo.Ejemplar.Disponible = true;
    }

}
