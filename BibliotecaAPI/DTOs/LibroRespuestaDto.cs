namespace BibliotecaAPI.DTOs;

public class LibroRespuestaDto
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public int CantidadEjemplares { get; set; }

    public int EjemplaresDisponibles { get; set; }
}
