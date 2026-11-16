namespace BibliotecaAPI.DTOs;

public class CrearLibroDto
{
    public string Titulo { get; set; } = string.Empty;

    public string Autor { get; set; } = string.Empty;

    public int CantidadEjemplares { get; set; } = 1;
}
