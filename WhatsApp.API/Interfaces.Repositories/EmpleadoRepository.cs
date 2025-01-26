using Microsoft.EntityFrameworkCore;
using WhatsApp.API.Context;
using WhatsApp.API.Models;

namespace WhatsApp.API.Interfaces.Repositories;

public class EmpleadoRepository : IEmpleadoRepository
{
    private readonly AppDbContext _context;

    public EmpleadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Empleado>> GetAllAsync()
    {
        return await _context.Empleados.ToListAsync();
    }

    public async Task<Empleado> GetByIdAsync(int id)
    {
        return await _context.Empleados.FindAsync(id);
    }

    public async Task AddAsync(Empleado empleado)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Verificar si el correo ya está registrado
            bool emailExists = await _context.Empleados
                .AnyAsync(e => e.Email == empleado.Email);

            if (emailExists)
            {
                throw new Exception($"El correo {empleado.Email} ya está registrado.");
            }

            await _context.Empleados.AddAsync(empleado);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(Empleado empleado)
    {
        var trackedEntity = await _context.Empleados.FindAsync(empleado.Id);
        if (trackedEntity != null)
        {
            _context.Entry(trackedEntity).CurrentValues.SetValues(empleado);
        }
        else
        {
            _context.Empleados.Update(empleado);
        }
        await _context.SaveChangesAsync();
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado == null)
        {
            return false;  // Empleado no encontrado
        }

        _context.Empleados.Remove(empleado);
        await _context.SaveChangesAsync();
        return true;  // Empleado eliminado
    }
}
