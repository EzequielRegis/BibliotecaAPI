using BibliotecaAPI.Datos;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController : ControllerBase
{
    private readonly BibliotecaContext _context;

    public PrestamosController(BibliotecaContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult RegistrarPrestamo(RegistrarPrestamoDto dto)
    {
        var libro = _context.Libros.Find(dto.LibroId);

        if (libro == null)
        {
            return NotFound("El libro no existe.");
        }

        var lector = _context.Lectores.Find(dto.LectorId);

        if (lector == null)
        {
            return NotFound("El lector no existe.");
        }

        var bibliotecario = _context.Bibliotecarios.Find(dto.BibliotecarioId);

        if (bibliotecario == null)
        {
            return NotFound("El bibliotecario no existe.");
        }

        if (!libro.Disponible)
        {
            return BadRequest("El libro no está disponible.");
        }

        var prestamo = new Prestamo
        {
            LibroId = dto.LibroId,
            LectorId = dto.LectorId,
            BibliotecarioId = dto.BibliotecarioId,
            FechaPrestamo = DateTime.Now,
            
        };

        libro.Disponible = false;

        _context.Prestamos.Add(prestamo);
        _context.SaveChanges();

        return Ok(new
        {
            prestamo.Id,
            prestamo.LibroId,
            prestamo.LectorId,
            prestamo.BibliotecarioId,
            prestamo.FechaPrestamo
        });
    }
}