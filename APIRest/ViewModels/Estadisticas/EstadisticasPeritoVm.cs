using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class EstadisticasPeritoVm : EstadisticasUsuarioVm
    {        
        public List<DetalleEstadisticaVm> NumSiniestrosCerrarPorAseguradora { get; set; }
    }
}
