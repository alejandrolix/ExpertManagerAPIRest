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
using APIRest.Filtros.ActionFilters;

namespace APIRest
{
    public class ComprobarToken : IActionFilter
    {
        private RutaExcluida[] RutasExlucidas { get; set; }

        private enum CodigoRespuesta
        {
            SesionExpirada = 0,
            NoInicioSesion = 1
        }

        public ComprobarToken()
        {
            RutasExlucidas = new RutaExcluida[]
            {
                new RutaExcluida(TipoMetodo.GET, "IniciarSesion"),
                new RutaExcluida(TipoMetodo.POST, "/api/Usuarios")
            };
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string metodoHttp = context.HttpContext.Request.Method;
            bool existeMetodo = RutasExlucidas.Any(rutaExcluida => Enum.IsDefined(typeof(TipoMetodo), metodoHttp));

            if (existeMetodo)
            {
                string urlPeticion = context.HttpContext.Request.Path.Value;
                bool existeRuta    = RutasExlucidas.Any(rutaExcluida => urlPeticion.Contains(rutaExcluida.Ruta));

                if (existeRuta)
                {
                    return;
                }
            }          

            string cabeceraAutorizacion = context.HttpContext.Request.Headers["Authorization"];
            string token                = cabeceraAutorizacion?.Split(" ")[1];

            if (string.IsNullOrEmpty(token))
            {
                EnviarMensajeRespuesta(context, "No ha iniciado sesión. Por favor, inicie sesión", CodigoRespuesta.NoInicioSesion);

                return;
            }

            RepositorioTokensUsuario repositorioTokensUsuario = (RepositorioTokensUsuario) context.HttpContext
                                                                                                  .RequestServices
                                                                                                  .GetService(typeof(RepositorioTokensUsuario));

            TokenUsuario tokenUsuario = repositorioTokensUsuario.ObtenerDatosToken(token)
                                                                .GetAwaiter()
                                                                .GetResult();     
            
            if (DateTime.Now > tokenUsuario.FechaHasta)
            {
                EnviarMensajeRespuesta(context, "Sesión expirada. Por favor, inicie sesión", CodigoRespuesta.SesionExpirada);
            }
        }

        private void EnviarMensajeRespuesta(ActionExecutingContext context, string mensaje, CodigoRespuesta codigoRespuesta)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                codigoRespuesta = (int) codigoRespuesta,
                error = mensaje
            });
        }
    }
}
