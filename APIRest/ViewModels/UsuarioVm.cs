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
        public int IdEsPerito { get; set; }
        public string EsPerito { get; set; }
        public int IdPermiso { get; set; }
        public string Permiso { get; set; }
        public string HashContrasenia { get; set; }

        public UsuarioVm(Usuario usuario)
        {
            string esPerito = "";

            if (usuario.EsPerito.HasValue)
                if (usuario.EsPerito.Value)
                    esPerito = "Sí";
                else
                    esPerito = "No";

            Id = usuario.Id;
            Nombre = usuario.Nombre;
            EsPerito = esPerito;
            IdPermiso = usuario.Permiso.Id;
            Permiso = usuario.Permiso.Nombre;
        }
    }
}
