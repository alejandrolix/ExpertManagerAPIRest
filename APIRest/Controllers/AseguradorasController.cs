using APIRest.Context;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.Repositorios;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradorasController : ControllerBase
    {
        private ExpertManagerContext _contexto;
        private RepositorioAseguradoras _repositorioAseguradoras;

        public AseguradorasController(ExpertManagerContext contexto, RepositorioAseguradoras repositorioAseguradoras)
        {
            _contexto = contexto;
            _repositorioAseguradoras = repositorioAseguradoras;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Aseguradora> aseguradoras = await _repositorioAseguradoras.ObtenerTodas();

            if (aseguradoras.Count == 0)                
                return StatusCode(500, "No existen aseguradoras");                                    

            return Ok(aseguradoras);
        }        
    }
}
