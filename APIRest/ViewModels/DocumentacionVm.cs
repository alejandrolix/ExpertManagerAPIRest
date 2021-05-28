using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.ViewModels
{
    public class DocumentacionVm
    {
        protected int Id { get; set; }
        protected string Descripcion { get; set; }
        protected int IdSiniestro { get; set; }
        protected IFormFile Archivo { get; set; }
    }
}
