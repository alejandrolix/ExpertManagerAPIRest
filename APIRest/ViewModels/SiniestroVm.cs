using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class SiniestroVm
    {
        public int Id { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public int IdAseguradora { get; set; }
        public string Aseguradora { get; set; }
        public string Descripcion { get; set; }
        public int IdPerito { get; set; }
        public string Perito { get; set; }
        public string FechaHoraAlta { get; set; }
        public int IdSujetoAfectado { get; set; }
        public string SujetoAfectado { get; set; }
        public int IdDanio { get; set; }
        public string Danio { get; set; }
        public string ImpValoracionDanios { get; set; }
    }
}
