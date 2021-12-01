using APIRest.Excepciones;
using APIRest.Filtros.Clases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace APIRest
{
    public class GeneradorErrorRespuesta : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            CodigoErrorHttpException excepcion = (CodigoErrorHttpException)context.Exception;
            RespuestaExcepcion respuestaExcepcion = new RespuestaExcepcion()
            {
                Error = context.Exception.Message
            };

            if (excepcion.CodigoErrorHttp == HttpStatusCode.NotFound)            
                context.Result = new NotFoundObjectResult(respuestaExcepcion);
            else
                context.Result = new ObjectResult(respuestaExcepcion)
                {
                    StatusCode = 500
                };
        }
    }
}
