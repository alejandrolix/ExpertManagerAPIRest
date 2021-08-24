using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace APIRest
{
    public class ComprobarToken : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string urlPeticion = context.HttpContext.Request.Path.Value;

            if (urlPeticion.Contains("api/Usuarios/IniciarSesion"))     // Ignoramos la ruta de iniciar sesión. Es decir, no se comprueba si existe token.
                return;            

            string cabeceraAutorizacion = context.HttpContext.Request.Headers["Authorization"];
            string token = cabeceraAutorizacion?.Split(" ")[1];

            if (string.IsNullOrEmpty(token))
                context.Result = new BadRequestObjectResult("no token");
        }
    }
}
