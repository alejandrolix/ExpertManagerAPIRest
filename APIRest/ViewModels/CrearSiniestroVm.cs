using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class CrearSiniestroVm
    {
        public int IdUsuarioAlta { get; set; }
        public int IdAseguradora { get; set; }
        public int IdEstado { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
        public int IdDanio { get; set; }
        public int IdSujetoAfectado { get; set; }
        public int IdPerito { get; set; }        
    }
}
