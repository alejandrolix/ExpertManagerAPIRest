using APIRest.Context;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class AseguradorasController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public AseguradorasController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: AseguradorasController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Aseguradora> aseguradoras = await _contexto.Aseguradoras                                                        
                                                            .ToListAsync();            
            if (aseguradoras.Count == 0)                
                return StatusCode(500, "No existen aseguradoras");                                    

            return Ok(aseguradoras);
        }        
    }
}
