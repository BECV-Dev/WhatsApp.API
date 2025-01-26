using Microsoft.EntityFrameworkCore;
using WhatsApp.API.Context;
using WhatsApp.API.Interfaces.Repositories;
// using WhatsApp.API.Middleware;
using WhatsApp.API.Service;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();

// Configuración de la conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configuración de Swagger/OpenAPI (opcional, pero útil para pruebas)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Permitir solicitudes desde cualquier origen
                .AllowAnyMethod()  // Permitir cualquier método HTTP (GET, POST, etc.)
                .AllowAnyHeader(); // Permitir cualquier encabezado
        });
});

// Interfaces, Repositorios y Servicios agregados
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<EmpleadoService>();

// Logs en base de datos
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<LogService>();

var app = builder.Build();

// Middleware de excepciones
// app.UseMiddleware<ExceptionMiddleware>();

// Configuración del pipeline de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();