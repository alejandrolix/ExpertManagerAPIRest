using System;

namespace APIRest.Excepciones
{
    public class CodigoErrorHttpException : Exception
    {
        public int CodigoErrorHttp { get; set; }

        public CodigoErrorHttpException(string mensaje, int codigoErrorHttp) : base(mensaje)
        {
            CodigoErrorHttp = codigoErrorHttp;
        }
    }
}
