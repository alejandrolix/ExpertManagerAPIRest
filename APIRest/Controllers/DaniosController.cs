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
    public class DaniosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public DaniosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: DaniosController
        [HttpGet]
        public async Task<List<Danio>> Index()
        {
            List<Danio> danios = await _contexto.Danios                                                        
                                                .ToListAsync();

            return danios;
        }        
    }
}
