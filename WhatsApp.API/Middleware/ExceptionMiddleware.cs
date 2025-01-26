// using System.Net;
// using System.Text.Json;
// using WhatsApp.API.Service;
//
// namespace WhatsApp.API.Middleware;
//
// public class ExceptionMiddleware
// {
//     private readonly RequestDelegate _next;
//     private readonly ILogger<ExceptionMiddleware> _logger;
//     private readonly IServiceProvider _serviceProvider; // Inyectar IServiceProvider
//     private readonly IWebHostEnvironment _env; // Obtener el entorno (Development or Production)
//
//     public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IServiceProvider serviceProvider, IWebHostEnvironment env)
//     {
//         _next = next;
//         _logger = logger;
//         _serviceProvider = serviceProvider;
//         _env = env;
//     }
//
//     public async Task InvokeAsync(HttpContext context)
//     {
//         try
//         {
//             // Mensaje de log para asegurarse de que el middleware es ejecutado correctamente
//             _logger.LogInformation("Iniciando procesamiento de solicitud.");
//             await _next(context);
//         }
//         catch (Exception ex)
//         {
//             using (var scope = _serviceProvider.CreateScope()) // Crear un alcance
//             {
//                 var logService = scope.ServiceProvider.GetRequiredService<LogService>(); // Obtener el LogService desde el alcance
//                 await logService.LogErrorAsync("Excepción no controlada", ex.Message);
//             }
//         
//             var innerException = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
//             _logger.LogError($"Algo salió mal: {ex.Message} - Inner Exception: {innerException}");
//         
//             await HandleExceptionAsync(context, ex);
//         }
//     }
//
//
//     private Task HandleExceptionAsync(HttpContext context, Exception exception)
//     {
//         context.Response.ContentType = "application/json";
//
//         var (statusCode, message) = exception switch
//         {
//             ArgumentNullException => (HttpStatusCode.BadRequest, "Uno o más argumentos no son válidos."),
//             KeyNotFoundException => (HttpStatusCode.NotFound, "El recurso solicitado no fue encontrado."),
//             UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "No tiene autorización para realizar esta acción."),
//             _ => (HttpStatusCode.InternalServerError, "Se produjo un error inesperado en el servidor.")
//         };
//
//         context.Response.StatusCode = (int)statusCode;
//         
//         var response = new
//         {
//             statusCode = context.Response.StatusCode,
//             message,
//             // Solo incluir los detalles de la excepción si estamos en desarrollo
//             details = _env.IsDevelopment() ? exception.Message : null
//         };
//
//         var jsonResponse = JsonSerializer.Serialize(response);
//
//         return context.Response.WriteAsync(jsonResponse);
//     }
// }