using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIRest.Repositorios;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradorasController : ControllerBase
    {        
        private RepositorioAseguradoras _repositorioAseguradoras;

        public AseguradorasController(RepositorioAseguradoras repositorioAseguradoras)
        {            
            _repositorioAseguradoras = repositorioAseguradoras;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Aseguradora> aseguradoras = await _repositorioAseguradoras.ObtenerTodas();                                            

            return Ok(aseguradoras);
        }        
    }
}
