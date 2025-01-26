using WhatsApp.API.Context;
using WhatsApp.API.Models;

namespace WhatsApp.API.Interfaces.Repositories;

public class LogRepository : ILogRepository
{
    private readonly AppDbContext _context;

    public LogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Log log)
    {
        // Añadir un mensaje de depuración para verificar si el log se está insertando correctamente
        Console.WriteLine($"Añadiendo log a la base de datos: {log.Message}");
        
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}