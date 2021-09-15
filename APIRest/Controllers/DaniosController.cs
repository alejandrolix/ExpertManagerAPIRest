using APIRest.Context;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.Repositorios;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class DaniosController : ControllerBase
    {
        private RepositorioDanios _repositorioDanios;

        public DaniosController(RepositorioDanios repositorioDanios)
        {
            _repositorioDanios = repositorioDanios;
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Danio> danios = await _repositorioDanios.ObtenerTodos();

            if (danios is null || danios.Count == 0)
                return StatusCode(500, "No existen daños");

            return Ok(danios);
        }        
    }
}
