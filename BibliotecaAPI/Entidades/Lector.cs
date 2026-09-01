namespace BibliotecaAPI.Entidades;

public class Lector
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public string DNI { get; set; }

    public ICollection<Prestamo> Prestamos { get; set; }
}
