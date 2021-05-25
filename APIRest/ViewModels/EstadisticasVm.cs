using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class EstadisticasVm
    {
        public int NumSiniestros { get; set; }
        public List<Tuple<string, int>> NumSiniestrosPorAseguradora { get; set; }
        public List<Tuple<string, int>> NumSiniestrosCerrarPorAseguradora { get; set; }
    }
}
