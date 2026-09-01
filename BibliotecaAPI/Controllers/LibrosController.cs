using BibliotecaAPI.Datos;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Entidades;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly BibliotecaContext _context;

    public LibrosController(BibliotecaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetLibros()
    {
        var libros = _context.Libros.ToList();

        return Ok(libros);
    }

    [HttpGet("{id}")]
    public IActionResult GetLibro(int id)
    {
        var libro = _context.Libros.Find(id);

        if (libro == null)
        {
            return NotFound();
        }

        return Ok(libro);
    }

    [HttpPost]
    public IActionResult CrearLibro(Libro libro)
    {
        _context.Libros.Add(libro);
        _context.SaveChanges();

        return CreatedAtAction(
            nameof(GetLibro),
            new { id = libro.Id },
            libro
        );
    }
}