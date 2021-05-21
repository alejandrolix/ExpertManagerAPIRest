using APIRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class UsuarioVm
    {
        public int Id { get; set; }        
        public string Nombre { get; set; }
        public string EsPerito { get; set; }
        public int IdPermiso { get; set; }
        public string Permiso { get; set; }
        public string HashContrasenia { get; set; }
        public decimal ImpReparacionDanios { get; set; }
    }
}
