using WhatsApp.API.Models;

namespace WhatsApp.API.Interfaces.Repositories;

public interface ILogRepository
{
    Task AddAsync(Log log);
}