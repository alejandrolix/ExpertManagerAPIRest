using APIRest.Context;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public EstadosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: EstadosController
        [HttpGet]
        public async Task<List<Estado>> Index()
        {
            List<Estado> siniestros = await _contexto.Estados                                                        
                                                     .ToListAsync();

            return siniestros;
        }        
    }
}
