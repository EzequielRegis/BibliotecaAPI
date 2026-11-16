namespace BibliotecaAPI.Client.DTOs;

public class PrestamoDto
{
    public int Id { get; set; }

    public int LibroId { get; set; }
    public string TituloLibro { get; set; } = "";

    public int LectorId { get; set; }
    public string NombreLector { get; set; } = "";

    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucionPrevista { get; set; }
    public DateTime? FechaDevolucionReal { get; set; }

    public bool EsDevuelto { get; set; }
    public decimal Multa { get; set; }
}
