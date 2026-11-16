using BibliotecaAPI.Entidades;
using System.ComponentModel.DataAnnotations;

public class Lector : EntidadBase
{
    public string Nombre { get; set; } = null!;

    [Required]
    [StringLength(20, MinimumLength = 7, ErrorMessage = "El DNI debe tener entre 7 y 20 caracteres.")]
    public string DNI { get; set; } = null!;

    [Required]
    [StringLength(15, ErrorMessage = "El teléfono no puede superar los 15 caracteres.")]
    public string Telefono { get; set; } = null!;

    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
