using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Models
{
    public class Permiso
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
    }
}
