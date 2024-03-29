﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Models
{
    public class Archivo
    {
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string UrlArchivo { get; set; }

        public int TipoArchivoId { get; set; }
        public TipoArchivo TipoArchivo { get; set; }        

        public int SiniestroId { get; set; }
        public Siniestro Siniestro { get; set; }
    }
}
