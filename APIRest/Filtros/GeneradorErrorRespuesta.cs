using APIRest.Excepciones;
using APIRest.Filtros.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace APIRest
{
    public class GeneradorErrorRespuesta : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception excepcion = context.Exception;

            RespuestaExcepcion respuestaExcepcion = new RespuestaExcepcion()
            {
                Error = excepcion.Message
            };

            if (excepcion is not CodigoErrorHttpException)
            {
                AsignarRespuestaPorCodigoHttp(context, respuestaExcepcion, HttpStatusCode.InternalServerError);

                return;
            }

            CodigoErrorHttpException excepcionCodigoErrorHttp = (CodigoErrorHttpException) excepcion;

            if (excepcionCodigoErrorHttp.CodigoErrorHttp == HttpStatusCode.NotFound)
                AsignarRespuestaPorCodigoHttp(context, respuestaExcepcion, HttpStatusCode.NotFound);
            else
                AsignarRespuestaPorCodigoHttp(context, respuestaExcepcion, HttpStatusCode.InternalServerError);
        }

        private void AsignarRespuestaPorCodigoHttp(ExceptionContext context, RespuestaExcepcion respuestaExcepcion, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.NotFound)
            {
                context.Result = new NotFoundObjectResult(respuestaExcepcion);
                
                return;
            }

            context.Result = new ObjectResult(respuestaExcepcion)
            {
                StatusCode = 500
            };
        }
    }
}
