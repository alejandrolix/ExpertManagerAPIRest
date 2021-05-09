using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class SiniestroVm
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Aseguradora { get; set; }
        public string Descripcion { get; set; }
        public string Perito { get; set; }
        public string FechaHoraAlta { get; set; }
        public string SujetoAfectado { get; set; }
        public string Danio { get; set; }
        public string ImpValoracionDanios { get; set; }
    }
}
