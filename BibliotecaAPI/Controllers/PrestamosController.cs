using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaAPI.Datos;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoRepository _repository;
    private readonly BibliotecaContext _context;

    public PrestamosController(
        IPrestamoRepository repository,
        BibliotecaContext context)
    {
        _repository = repository;
        _context = context;
    }

    // Obtener todos los préstamos
    [HttpGet]
    public IActionResult GetPrestamos()
    {
        var prestamos = _repository.ObtenerPrestamos();

        var respuesta = prestamos.Select(p => new PrestamoRespuestaDto
        {
            Id = p.Id,
            LibroId = p.Ejemplar?.LibroId ?? 0,
            TituloLibro = p.Ejemplar?.Libro?.Titulo ?? "Sin título",
            LectorId = p.LectorId,
            NombreLector = p.Lector?.Nombre ?? "Sin nombre",
            FechaPrestamo = p.FechaPrestamo,
            FechaDevolucionPrevista = p.FechaDevolucionPrevista,
            FechaDevolucionReal = p.FechaDevolucionReal,
            EsDevuelto = p.EsDevuelto,
            Multa = p.Multa ?? 0m
        });

        return Ok(respuesta);
    }

    // Registrar un préstamo individual [CASO DE USO]
    [HttpPost]
    public IActionResult RegistrarPrestamo(RegistrarPrestamoDto dto)
    {
        var libro = _repository.ObtenerLibro(dto.LibroId);

        if (libro == null)
        {
            return NotFound("El libro no existe.");
        }

        var lector = _repository.ObtenerLector(dto.LectorId);

        if (lector == null)
        {
            return NotFound("El lector no existe.");
        }

        var ejemplar = _repository.ObtenerEjemplarDisponible(dto.LibroId);

        if (ejemplar == null)
        {
            return BadRequest("No hay ejemplares disponibles de este libro.");
        }

        var fechaPrestamo = DateTime.Now;

        var prestamo = new Prestamo
        {
            EjemplarId = ejemplar.Id,
            LectorId = dto.LectorId,
            FechaPrestamo = fechaPrestamo,
            FechaDevolucionPrevista = fechaPrestamo.AddDays(14),
            EsDevuelto = false
        };

        _repository.ActualizarDisponibilidad(ejemplar);
        _repository.AgregarPrestamo(prestamo);
        _repository.GuardarCambios();

        return Ok(new
        {
            prestamo.Id,
            prestamo.EjemplarId,
            prestamo.LectorId,
            prestamo.FechaPrestamo,
            prestamo.FechaDevolucionPrevista
        });
    }

    // Registrar varios préstamos en una operación
    [HttpPost("multiple")]
    public IActionResult RegistrarPrestamosMultiples(
        RegistrarPrestamoMultipleDto dto)
    {
        if (dto.Libros == null || dto.Libros.Count == 0)
        {
            return BadRequest("Tenés que seleccionar al menos un libro.");
        }

        if (dto.Libros.Any(l => l.Cantidad < 1))
        {
            return BadRequest(
                "La cantidad de ejemplares debe ser al menos 1.");
        }
        if (dto.Libros.Any(l => l.LibroId <= 0))
        {
            return BadRequest(
                "El identificador de cada libro debe ser mayor que 0.");
        }
        // Agrupar libros repetidos y sumar sus cantidades
        var cantidadesPorLibro = dto.Libros
            .GroupBy(l => l.LibroId)
            .ToDictionary(g => g.Key, g => g.Sum(l => l.Cantidad));

        var lector = _repository.ObtenerLector(dto.LectorId);

        if (lector == null)
        {
            return NotFound("El lector no existe.");
        }

        var ejemplaresSeleccionados = new List<Ejemplar>();
        var avisos = new List<string>();

        // Validar todos los libros antes de registrar préstamos
        foreach (var item in cantidadesPorLibro)
        {
            var libro = _repository.ObtenerLibro(item.Key);

            if (libro == null)
            {
                avisos.Add($"No existe el libro con ID {item.Key}.");
                continue;
            }

            var ejemplaresDisponibles =
    _repository.ObtenerEjemplaresDisponibles(item.Key);

            if (ejemplaresDisponibles.Count < item.Value)
            {
                avisos.Add(
                    $"Se solicitaron {item.Value} ejemplares de '{libro.Titulo}', " +
                    $"pero solo hay {ejemplaresDisponibles.Count} disponibles.");
            }

            ejemplaresSeleccionados.AddRange(
                ejemplaresDisponibles.Take(item.Value));
        }
        if (ejemplaresSeleccionados.Count == 0)
        {
            return BadRequest(
                "No se pudo registrar ningún préstamo porque no hay ejemplares disponibles.");
        }

        try
        {
            using var transaccion = _context.Database.BeginTransaction();

            // Registrar un préstamo por cada ejemplar seleccionado
            var fechaPrestamo = DateTime.Now;

            foreach (var ejemplar in ejemplaresSeleccionados)
            {
                var prestamo = new Prestamo
                {
                    EjemplarId = ejemplar.Id,
                    LectorId = dto.LectorId,
                    FechaPrestamo = fechaPrestamo,
                    FechaDevolucionPrevista = fechaPrestamo.AddDays(14),
                    EsDevuelto = false
                };

                _repository.ActualizarDisponibilidad(ejemplar);
                _repository.AgregarPrestamo(prestamo);
            }

            _repository.GuardarCambios();
            transaccion.Commit();
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Error al registrar préstamos múltiples: {ex}");

            return StatusCode(
                500,
                "Ocurrió un error al registrar los préstamos. No se guardaron los cambios.");
        }


        return Ok(new
        {
            Mensaje = avisos.Count == 0
         ? "Préstamos registrados correctamente."
         : "Préstamos registrados con observaciones.",
            LectorId = dto.LectorId,
            CantidadEjemplares = ejemplaresSeleccionados.Count,
            Avisos = avisos
        });
    }

    // Registrar la devolución de un préstamo
    [HttpPost("{id}/devolver")]
    public IActionResult DevolverPrestamo(
        int id,
        [FromBody] DevolverPrestamoDto dto)
    {
        var prestamo = _repository.ObtenerPrestamo(id);

        if (prestamo == null)
        {
            return NotFound("El préstamo no existe.");
        }

        if (prestamo.EsDevuelto)
        {
            return BadRequest("El libro ya fue devuelto.");
        }

        try
        {
            DateTime fechaDevolucion =
                dto.FechaDevolucionReal == default
                    ? DateTime.Now
                    : dto.FechaDevolucionReal;

            if (fechaDevolucion > DateTime.Now)
            {
                return BadRequest(
                    "La fecha de devolución no puede ser futura.");
            }

            if (fechaDevolucion < prestamo.FechaPrestamo)
            {
                return BadRequest(
                    "La fecha de devolución no puede ser anterior al préstamo.");
            }

            _repository.RegistrarDevolucion(
                prestamo,
                fechaDevolucion);

            _repository.GuardarCambios();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(new
        {
            prestamo.Id,
            prestamo.FechaDevolucionReal,
            prestamo.Multa
        });
    }
}
