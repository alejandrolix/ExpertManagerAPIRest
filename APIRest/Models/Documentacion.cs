using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Models
{
    public class Documentacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int SiniestroId { get; set; }
        public Siniestro Siniestro { get; set; }
    }
}
