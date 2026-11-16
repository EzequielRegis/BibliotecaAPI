using BibliotecaAPI.Entidades;

namespace BibliotecaAPI.Repositories;

public interface ILectorRepository
{
    void AgregarLector(Lector lector);
    void EliminarLector(Lector lector);
    bool TienePrestamos(int lectorId);
    Lector? ObtenerLector(int id);

    Lector? ObtenerLectorPorDNI(string dni);
    List<Lector> ObtenerLectores();
    void GuardarCambios();
}
