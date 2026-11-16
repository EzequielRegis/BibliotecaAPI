using BibliotecaAPI.Entidades;
using BibliotecaAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LectoresController : ControllerBase
{
    private readonly ILectorRepository _repository;

    public LectoresController(ILectorRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetLectores()
    {
        var lectores = _repository.ObtenerLectores();

        return Ok(lectores);
    }

    [HttpGet("{id}")]
    public IActionResult GetLector(int id)
    {
        var lector = _repository.ObtenerLector(id);

        if (lector == null)
        {
            return NotFound("El lector no existe.");
        }

        return Ok(lector);
    }


    [HttpPost]
    public IActionResult RegistrarLector(Lector lector)
    {
        if (string.IsNullOrWhiteSpace(lector.Nombre))
        {
            return BadRequest("Ingresá el nombre del lector.");
        }

        if (string.IsNullOrWhiteSpace(lector.DNI))
        {
            return BadRequest("Ingresá el DNI del lector.");
        }

        if (string.IsNullOrWhiteSpace(lector.Telefono))
        {
            return BadRequest("Ingresá el teléfono del lector.");
        }
        if (lector.DNI.Length < 7 || lector.DNI.Length > 20)
        {
            return BadRequest("El DNI debe tener entre 7 y 20 caracteres.");
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(
        lector.DNI, @"^[a-zA-Z0-9]+$"))
        {
            return BadRequest(
                "El documento debe contener solamente letras y números.");
        }
        if (lector.Telefono.Length > 15)
        {
            return BadRequest("El teléfono no puede superar los 15 caracteres.");
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(
        lector.Telefono, @"^\+?[0-9]+$"))
        {
            return BadRequest(
                "El teléfono debe contener solamente números y puede comenzar con +.");
        }
        var lectorExistente = _repository.ObtenerLectorPorDNI(lector.DNI);

        if (lectorExistente != null)
        {
            return BadRequest("Ya existe un lector registrado con ese DNI.");
        }

        _repository.AgregarLector(lector);
        _repository.GuardarCambios();

        return Ok(lector);
    }
   
    
    [HttpDelete("{id}")]
    public IActionResult EliminarLector(int id)
    {
        var lector = _repository.ObtenerLector(id);

        if (lector == null)
        {
            return NotFound("El lector no existe.");
        }

        if (_repository.TienePrestamos(id))
        {
            return BadRequest(
                "No se puede eliminar este lector porque tiene préstamos registrados.");
        }

        _repository.EliminarLector(lector);
        _repository.GuardarCambios();

        return Ok("Lector eliminado correctamente.");
    }

}
