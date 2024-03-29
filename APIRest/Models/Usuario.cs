﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column(TypeName = "decimal(5,2)")]
        public decimal ImpRepacionDanios { get; set; }
        
        public int PermisoId { get; set; }
        public Permiso Permiso { get; set; }
    }
}
