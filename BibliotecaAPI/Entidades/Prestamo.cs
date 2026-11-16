
namespace BibliotecaAPI.Entidades;

public class Prestamo : EntidadBase
{
    public int EjemplarId { get; set; }
    public Ejemplar Ejemplar { get; set; } = null!; 

    public int LectorId { get; set; }
    public Lector Lector { get; set; } = null!;

    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucionPrevista { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }

    public bool EsDevuelto { get; set; } = false;

    public decimal? Multa { get; set; }
}
