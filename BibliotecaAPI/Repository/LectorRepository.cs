using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;

namespace BibliotecaAPI.Repositories;

public class LectorRepository : ILectorRepository
{
    private readonly BibliotecaContext _context;

    public LectorRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public void AgregarLector(Lector lector)
    {
        _context.Lectores.Add(lector);
    }
    public void EliminarLector(Lector lector)
    {
        _context.Lectores.Remove(lector);
    }
    public bool TienePrestamos(int lectorId)
    {
        return _context.Prestamos
            .Any(p => p.LectorId == lectorId);
    }
    public Lector? ObtenerLector(int id)
    {
        return _context.Lectores.Find(id);
    }
    public Lector? ObtenerLectorPorDNI(string dni)
    {
        return _context.Lectores
            .FirstOrDefault(l => l.DNI == dni);
    }
    public List<Lector> ObtenerLectores()
    {
        return _context.Lectores.ToList();
    }

    public void GuardarCambios()
    {
        _context.SaveChanges();
    }
}
