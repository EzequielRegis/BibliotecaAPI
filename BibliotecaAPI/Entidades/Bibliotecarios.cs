namespace BibliotecaAPI.Entidades;

public class Bibliotecario
{
    public int Id { get; set; }

    public string Nombre { get; set; }

    public ICollection<Prestamo> Prestamos { get; set; }
}
