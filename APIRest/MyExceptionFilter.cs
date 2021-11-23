using APIRest.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIRest
{
    public class MyExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            CodigoErrorHttpException excepcion = (CodigoErrorHttpException)context.Exception;
            var respuesta = new
            {
                error = context.Exception.Message
            };

            if (excepcion.CodigoErrorHttp == System.Net.HttpStatusCode.NotFound)            
                context.Result = new NotFoundObjectResult(respuesta);
            else
                context.Result = new ObjectResult(respuesta)
                {
                    StatusCode = 500
                };
        }
    }
}
