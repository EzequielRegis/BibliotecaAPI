namespace BibliotecaAPI.DTOs;

public class RegistrarPrestamoMultipleDto
{
    public int LectorId { get; set; }

    public List<LibroConCantidadDto> Libros { get; set; } = new();
}

public class LibroConCantidadDto
{
    public int LibroId { get; set; }

    public int Cantidad { get; set; }
}
