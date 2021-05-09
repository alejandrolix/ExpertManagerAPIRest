using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
        public async Task<List<Aseguradora>> Index()
        {
            List<Aseguradora> aseguradoras = await _contexto.Aseguradoras                                                        
                                                            .ToListAsync();

            return aseguradoras;
        }        
    }
}
