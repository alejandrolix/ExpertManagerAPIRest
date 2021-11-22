using System;
using System.Net;

namespace APIRest.Excepciones
{
    public class CodigoErrorHttpException : Exception
    {
        public HttpStatusCode CodigoErrorHttp { get; private set; }

        public CodigoErrorHttpException(string mensaje, HttpStatusCode codigoErrorHttp) : base(mensaje)
        {
            CodigoErrorHttp = codigoErrorHttp;
        }
    }
}
