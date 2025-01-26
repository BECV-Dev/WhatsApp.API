namespace WhatsApp.API.Models;

public class Log
{
    public int Id { get; set; }
    public string Level { get; set; } // Info, Warning, Error
    public string Message { get; set; }
    public string Details { get; set; }
    public DateTime Timestamp { get; set; }
}