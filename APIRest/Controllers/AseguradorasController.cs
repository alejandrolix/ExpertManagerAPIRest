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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Aseguradora aseguradora = await _repositorioAseguradoras.ObtenerPorId(id);

            try
            {
                await _repositorioAseguradoras.Eliminar(aseguradora);
            }
            catch (Exception)
            {
                return StatusCode(500, $"No se ha podido eliminar la aseguradora con id {id}");
            }

            return Ok(true);
        }
    }
}
