namespace BibliotecaAPI.DTOs;

public class PrestamoRespuestaDto
{
    public int Id { get; set; }

    public int LibroId { get; set; }
    public string TituloLibro { get; set; } = null!;

    public int LectorId { get; set; }
    public string NombreLector { get; set; } = null!;

    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucionPrevista { get; set; }
    public DateTime? FechaDevolucionReal { get; set; } = null!;

    public bool EsDevuelto { get; set; }
    public decimal Multa { get; set; }
}

