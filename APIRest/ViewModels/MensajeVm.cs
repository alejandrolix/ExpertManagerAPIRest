using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class MensajeVm
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuarioCreado { get; set; }
        public int IdSiniestro { get; set; }
        public string UsuarioCreado { get; set; }
    }
}
