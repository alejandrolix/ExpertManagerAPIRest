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

            if (excepcion is not CodigoErrorHttpException)
            {
                AsignarRespuestaPorCodigoHttp(context, excepcion.Message, HttpStatusCode.InternalServerError);

                return;
            }

            CodigoErrorHttpException excepcionCodigoErrorHttp = (CodigoErrorHttpException) excepcion;

            if (excepcionCodigoErrorHttp.CodigoErrorHttp == HttpStatusCode.NotFound)
            {
                AsignarRespuestaPorCodigoHttp(context, excepcion.Message, HttpStatusCode.NotFound);
            }
            else
            {
                AsignarRespuestaPorCodigoHttp(context, excepcion.Message, HttpStatusCode.InternalServerError);
            }
        }

        private void AsignarRespuestaPorCodigoHttp(ExceptionContext context, string mensaje, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.NotFound)
            {
                context.Result = new NotFoundObjectResult(mensaje);
            }
            else
            {
                context.Result = new ObjectResult(mensaje)
                {
                    StatusCode = 500
                };
            }
        }
    }
}
