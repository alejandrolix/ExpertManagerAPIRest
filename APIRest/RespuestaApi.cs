using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest
{
    public class RespuestaApi
    {
        public string Mensaje { get; set; }
        public int CodigoRespuesta { get; set; }
        public object Datos { get; set; }
    }
}
