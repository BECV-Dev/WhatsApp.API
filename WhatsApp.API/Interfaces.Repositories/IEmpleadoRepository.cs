using WhatsApp.API.Models;

namespace WhatsApp.API.Interfaces.Repositories;

public interface IEmpleadoRepository
{
    Task<IEnumerable<Empleado>> GetAllAsync();
    Task<Empleado> GetByIdAsync(int id);
    Task AddAsync(Empleado empleado);
    Task UpdateAsync(Empleado empleado);
    Task<bool> DeleteAsync(int id);  // Cambio aquí
}