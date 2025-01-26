using WhatsApp.API.Interfaces.Repositories;
using WhatsApp.API.Models;

namespace WhatsApp.API.Service;

public class LogService
{
    private readonly ILogRepository _logRepository;

    public LogService(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public async Task LogErrorAsync(string message, string details)
    {
        var log = new Log
        {
            Level = "Error",
            Message = message,
            Details = details,
            Timestamp = DateTime.UtcNow
        };
        await _logRepository.AddAsync(log);
    }

    public async Task LogInfoAsync(string message, string details)
    {
        var log = new Log
        {
            Level = "Info",
            Message = message,
            Details = details,
            Timestamp = DateTime.UtcNow
        };
        await _logRepository.AddAsync(log);
    }
    
    public async Task LogWarningAsync(string message, string details)
    {
        var log = new Log
        {
            Level = "Warning",
            Message = message,
            Details = details,
            Timestamp = DateTime.UtcNow
        };
        await _logRepository.AddAsync(log);
    }
}
