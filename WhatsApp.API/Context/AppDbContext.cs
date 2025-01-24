using Microsoft.EntityFrameworkCore;
using WhatsApp.API.Models;

namespace WhatsApp.API.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Empleado> Empleados { get; set; }
}