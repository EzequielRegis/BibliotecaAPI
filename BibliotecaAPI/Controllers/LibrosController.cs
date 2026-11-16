using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly ILibroRepository _repository;

    public LibrosController(ILibroRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetLibros()
    {
        var libros = _repository.ObtenerLibros();

        var respuesta = libros.Select(libro => new LibroRespuestaDto
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Autor = libro.Autor,
            CantidadEjemplares = libro.Ejemplares.Count,
            EjemplaresDisponibles = libro.Ejemplares.Count(e => e.Disponible)
        });

        return Ok(respuesta);
    }
    [HttpGet("{id}")]
    public IActionResult GetLibro(int id)
    {
        var libro = _repository.ObtenerLibro(id);

        if (libro == null)
        {
            return NotFound("Libro no encontrado.");
        }

        var respuesta = new LibroRespuestaDto
        {
            Id = libro.Id,
            Titulo = libro.Titulo,
            Autor = libro.Autor,
            CantidadEjemplares = libro.Ejemplares.Count,
            EjemplaresDisponibles = libro.Ejemplares.Count(e => e.Disponible)
        };

        return Ok(respuesta);
    }




    [HttpPost]
    public IActionResult CrearLibro(CrearLibroDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
        {
            return BadRequest("Ingresá el título del libro.");
        }

        if (dto.CantidadEjemplares < 1)
        {
            return BadRequest("Tenés que registrar al menos un ejemplar.");
        }

        string titulo = dto.Titulo.Trim();

        string autor = string.IsNullOrWhiteSpace(dto.Autor)
            ? "Autor desconocido"
            : dto.Autor.Trim();

        var libroExistente = _repository.ObtenerLibros()
            .FirstOrDefault(l =>
                l.Titulo.Trim().Equals(
                    titulo, StringComparison.OrdinalIgnoreCase) &&
                l.Autor.Trim().Equals(
                    autor, StringComparison.OrdinalIgnoreCase));

        if (libroExistente != null)
        {
            for (int i = 0; i < dto.CantidadEjemplares; i++)
            {
                libroExistente.Ejemplares.Add(new Ejemplar
                {
                    Disponible = true
                });
            }

            _repository.GuardarCambios();

            return Ok(new LibroRespuestaDto
            {
                Id = libroExistente.Id,
                Titulo = libroExistente.Titulo,
                Autor = libroExistente.Autor,
                CantidadEjemplares = libroExistente.Ejemplares.Count,
                EjemplaresDisponibles = libroExistente.Ejemplares.Count(
                    e => e.Disponible)
            });
        }

        var libro = new Libro
        {
            Titulo = titulo,
            Autor = autor
        };

        for (int i = 0; i < dto.CantidadEjemplares; i++)
        {
            libro.Ejemplares.Add(new Ejemplar
            {
                Disponible = true
            });
        }

        _repository.AgregarLibro(libro);
        _repository.GuardarCambios();

        return CreatedAtAction(
            nameof(GetLibro),
            new { id = libro.Id },
            new LibroRespuestaDto
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Autor = libro.Autor,
                CantidadEjemplares = libro.Ejemplares.Count,
                EjemplaresDisponibles = libro.Ejemplares.Count(
                    e => e.Disponible)
            });
    }
    [HttpDelete("{id}")]
    public IActionResult EliminarLibro(int id)
    {
        var libro = _repository.ObtenerLibro(id);

        if (libro == null)
        {
            return NotFound("Libro no encontrado.");
        }

        if (libro.Ejemplares.Any(e => e.Prestamos.Any()))
        {
            return BadRequest(
                "No se puede eliminar este libro porque tiene préstamos registrados."
            );
        }

        _repository.EliminarLibro(libro);
        _repository.GuardarCambios();

        return Ok("Libro eliminado correctamente.");
    }



}