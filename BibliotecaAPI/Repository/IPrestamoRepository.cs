using BibliotecaAPI.Entidades;

namespace BibliotecaAPI.Repositories;

public interface IPrestamoRepository
{
    Libro? ObtenerLibro(int id);
    Lector? ObtenerLector(int id);
    Ejemplar? ObtenerEjemplarDisponible(int libroId);
    List<Ejemplar> ObtenerEjemplaresDisponibles(int libroId);
    void AgregarPrestamo(Prestamo prestamo);
    void GuardarCambios();

    void ActualizarDisponibilidad(Ejemplar ejemplar);

    Prestamo? ObtenerPrestamo(int id);
    List<Prestamo> ObtenerPrestamos();
    void RegistrarDevolucion(Prestamo prestamo, DateTime fechaDevolucionReal);
}
