using APIRest.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace APIRest
{
    public class GeneradorErrorRespuesta : IExceptionFilter
    {
        private Exception Excepcion { get; set; }

        public void OnException(ExceptionContext context)
        {
            Excepcion = context.Exception;

            if (Excepcion is not CodigoErrorHttpException)
            {
                AsignarRespuestaPorCodigoHttp(context, HttpStatusCode.InternalServerError);

                return;
            }

            CodigoErrorHttpException excepcionCodigoErrorHttp = (CodigoErrorHttpException) Excepcion;

            if (excepcionCodigoErrorHttp.CodigoErrorHttp == HttpStatusCode.NotFound)
            {
                AsignarRespuestaPorCodigoHttp(context, HttpStatusCode.NotFound);
            }
            else
            {
                AsignarRespuestaPorCodigoHttp(context, HttpStatusCode.InternalServerError);
            }
        }

        private void AsignarRespuestaPorCodigoHttp(ExceptionContext context, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.NotFound)
            {
                context.Result = new NotFoundObjectResult(Excepcion.Message);
            }
            else
            {
                context.Result = new ObjectResult(Excepcion.Message)
                {
                    StatusCode = 500
                };
            }
        }
    }
}
