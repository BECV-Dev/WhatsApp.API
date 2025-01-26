using WhatsApp.API.Interfaces.Repositories;
using WhatsApp.API.Models;

namespace WhatsApp.API.Service;

public class EmpleadoService
{
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly LogService _logService;

    public EmpleadoService(IEmpleadoRepository empleadoRepository, LogService logService)
    {
        _empleadoRepository = empleadoRepository;
        _logService = logService;
    }

    public async Task<IEnumerable<Empleado>> GetAllEmpleadosAsync()
    {
        try
        {
            var empleados = await _empleadoRepository.GetAllAsync();
            if(empleados.Count() > 0)
            {
                await _logService.LogInfoAsync("EmpleadoService - GetAllEmpleados()", "Se recuperaron con éxito todos los empleados de la base de datos.");
            }
            else
            {
                await _logService.LogInfoAsync("EmpleadoService - GetAllEmpleados()", "No se encontraron empleados registrados en la base de datos.");
            }
            return empleados;
        }
        catch (Exception ex)
        {
            await _logService.LogErrorAsync("Error en la obtención de empleados", ex.Message);
            throw;
        }
    }

    public async Task<Empleado> GetEmpleadoByIdAsync(int id)
    {
        try
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null)
            {
                await _logService.LogWarningAsync("EmpleadoService - GetEmpleadoById()", $"No se encontró un empleado con ID {id} en la base de datos.");
            }
            else
            {
                await _logService.LogInfoAsync("EmpleadoService - GetEmpleadoById()", $"Empleado con ID {id} recuperado correctamente.");
            }
            return empleado;
        }
        catch (Exception ex)
        {
            await _logService.LogErrorAsync("Error al obtener empleado por ID", ex.Message);
            throw;
        }
    }

    public async Task AddEmpleadoAsync(Empleado empleado)
    {
        try
        {
            await _empleadoRepository.AddAsync(empleado);
            await _logService.LogInfoAsync("EmpleadoService - AddEmpleado()", $"Empleado con correo {empleado.Email} agregado exitosamente a la base de datos.");
        }
        catch (Exception ex)
        {
            await _logService.LogErrorAsync("Error al agregar nuevo empleado", ex.Message);
            throw;
        }
    }

    public async Task UpdateEmpleadoAsync(Empleado empleado)
    {
        try
        {
            await _logService.LogInfoAsync("EmpleadoService - UpdateEmpleado()", $"Iniciando actualización para el empleado con ID {empleado.Id}.");
            await _empleadoRepository.UpdateAsync(empleado);
            await _logService.LogInfoAsync("EmpleadoService - UpdateEmpleado()", $"Empleado con ID {empleado.Id} actualizado exitosamente en la base de datos.");
        }
        catch (Exception ex)
        {
            await _logService.LogErrorAsync("Error al actualizar empleado", ex.Message);
            throw;
        }
    }

    public async Task DeleteEmpleadoAsync(int id)
    {
        try
        {
            bool isDeleted = await _empleadoRepository.DeleteAsync(id);
            if (isDeleted)
            {
                await _logService.LogInfoAsync("EmpleadoService - DeleteEmpleado()", $"Empleado con ID {id} eliminado correctamente de la base de datos.");
            }
            else
            {
                await _logService.LogWarningAsync("EmpleadoService - DeleteEmpleado()", $"No se encontró un empleado con ID {id} para eliminar.");
            }
        }
        catch (Exception ex)
        {
            await _logService.LogErrorAsync("Error al eliminar empleado", ex.Message);
            throw;
        }
    }
}
