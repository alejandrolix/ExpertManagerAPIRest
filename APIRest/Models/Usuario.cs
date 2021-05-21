﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Contrasenia { get; set; }

        public bool? EsPerito { get; set; }
        
        public int PermisoId { get; set; }
        public Permiso Permiso { get; set; }

        public string ObtenerPeritoCadena(bool? esPerito)
        {
            if (esPerito.Value)
                return "Sí";
            else
                return "No";
        }
    }
}
