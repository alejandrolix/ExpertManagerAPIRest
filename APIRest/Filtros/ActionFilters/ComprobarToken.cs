using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using APIRest.Repositorios;
using APIRest.Models;
using APIRest.Excepciones;

namespace APIRest
{
    public class ComprobarToken : IActionFilter
    {
        private enum CodigoRespuesta
        {
            SesionExpirada = 0,
            NoInicioSesion = 1
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string urlPeticion = context.HttpContext.Request.Path.Value;

            if (urlPeticion.Contains("api/Usuarios/IniciarSesion"))     // Ignoramos la ruta de iniciar sesión.
                return;            

            string cabeceraAutorizacion = context.HttpContext.Request.Headers["Authorization"];
            string token = cabeceraAutorizacion?.Split(" ")[1];

            if (string.IsNullOrEmpty(token))
            {
                AsignarMensajeRespuesta(context, "No ha iniciado sesión. Por favor, inicie sesión", CodigoRespuesta.NoInicioSesion);

                return;
            }

            RepositorioTokensUsuario repositorioTokensUsuario = (RepositorioTokensUsuario) context.HttpContext
                                                                                                  .RequestServices
                                                                                                  .GetService(typeof(RepositorioTokensUsuario));

            TokenUsuario tokenUsuario = repositorioTokensUsuario.ObtenerDatosToken(token)
                                                                .GetAwaiter()
                                                                .GetResult();     
            
            if (DateTime.Now > tokenUsuario.FechaHasta)
                AsignarMensajeRespuesta(context, "Sesión expirada. Por favor, inicie sesión", CodigoRespuesta.SesionExpirada);
        }

        private void AsignarMensajeRespuesta(ActionExecutingContext context, string mensaje, CodigoRespuesta codigoRespuesta)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                codigoRespuesta = (int) codigoRespuesta,
                error = mensaje
            });
        }
    }
}
