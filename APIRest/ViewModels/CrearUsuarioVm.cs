using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class CrearUsuarioVm
    {
        public int IdPermiso { get; set; }
        public string Nombre { get; set; }
        public string HashContrasenia { get; set; }     
        public decimal ImpReparacionDanios { get; set; }
    }
}
