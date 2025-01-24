using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsApp.API.Models;
using WhatsApp.API.Context;

namespace WhatsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpleadoController(AppDbContext context)
        {
            _context = context;
        }

        // Obtener todos los empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            try
            {
                var empleados = await _context.Empleados.ToListAsync();
                return Ok(empleados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // Obtener un empleado por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            try
            {
                var empleado = await _context.Empleados.FindAsync(id);

                if (empleado == null)
                {
                    return NotFound();
                }

                return Ok(empleado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // Crear un nuevo empleado
        [HttpPost]
        public async Task<ActionResult<Empleado>> PostEmpleado(Empleado empleado)
        {
            try
            {
                // Verificamos si ya existe un empleado con el mismo email
                if (_context.Empleados.Any(e => e.Email == empleado.Email))
                {
                    return BadRequest("Ya existe un empleado con ese email.");
                }

                // Agregar el empleado
                _context.Empleados.Add(empleado);
                await _context.SaveChangesAsync();

                // Retornar el empleado insertado
                return CreatedAtAction("GetEmpleado", new { id = empleado.Id }, empleado);
            }
            catch (Exception ex)
            {
                // Si ocurre un error, devolverlo
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // Actualizar un empleado
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpleado(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return BadRequest();
            }

            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Eliminar un empleado
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
            {
                return NotFound();
            }

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verificar si el empleado existe
        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
