using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Models
{
    public class Documentacion
    {
        protected int Id { get; set; }

        [Required]
        protected string Descripcion { get; set; }

        [Required]
        protected string UrlArchivo { get; set; }

        protected int SiniestroId { get; set; }
        protected Siniestro Siniestro { get; set; }
    }
}
