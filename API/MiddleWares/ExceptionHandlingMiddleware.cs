using Newtonsoft.Json;
using Service.Exceptions;
using System.Net;

namespace API.MiddleWares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (InvalidUserException ex)
            {
                _logger.LogError(ex, "Utilisateur non trouvé.");
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                var response = new { message = "Utilisateur non trouvé." };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Une exception non gérée s'est produite.");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = new { message = "Une erreur interne est survenue. Veuillez réessayer plus tard." };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
