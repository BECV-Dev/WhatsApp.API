using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsApp.API.Models;
using WhatsApp.API.Service;

namespace WhatsApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoService _empleadoService;

        // Inyección de dependencias: el controlador recibe el servicio a través del constructor
        public EmpleadoController(EmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        // GET: api/empleado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            return Ok(empleados);
        }

        // GET: api/empleado/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            var arrayEmpleado = new Empleado
            {
                Id = empleado.Id,
                Nombre = empleado.Nombre
            };
            
            return Ok(arrayEmpleado);
        }

        // POST: api/empleado
        [HttpPost]
        public async Task<ActionResult<Empleado>> AddEmpleado(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve un error si el modelo no es válido
            }
            
            try
            {
                empleado.Nombre = null;
                await _empleadoService.AddEmpleadoAsync(empleado);
                return CreatedAtAction(nameof(GetEmpleado), new { id = empleado.Id }, empleado);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Error en la base de datos.", details = dbEx.Message });
            }
            catch (Exception ex)
            {
                // Aquí puedes loggear el error o agregar lógica específica de manejo
                return StatusCode(500, new { message = "Ocurrió un error inesperado al agregar el empleado.", details = ex.Message });
            }
        }


        // PUT: api/empleado/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmpleado(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return BadRequest();
            }
            
            var existingEmpleado = await _empleadoService.GetEmpleadoByIdAsync(id);
            if (existingEmpleado == null)
            {
                return NotFound(); // Devuelve 404 si el empleado no existe
            }
            
            await _empleadoService.UpdateEmpleadoAsync(empleado);
            return NoContent();
        }

        // DELETE: api/empleado/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            await _empleadoService.DeleteEmpleadoAsync(id);
            return NoContent();
        }
    }
}
