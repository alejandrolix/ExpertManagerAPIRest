using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class ImagenVm
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int IdSiniestro { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
