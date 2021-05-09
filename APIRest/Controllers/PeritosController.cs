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
    public class PeritosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public PeritosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: PeritosController
        [HttpGet]
        public async Task<List<PeritoVm>> Index()
        {
            List<Usuario> peritos = await _contexto.Usuarios
                                                   .Where(usuario => usuario.EsPerito.HasValue && usuario.EsPerito.Value)
                                                   .ToListAsync();

            List<PeritoVm> peritosVms = peritos.Select(perito => new PeritoVm()
            {
                Id = perito.Id,
                Nombre = perito.Nombre
            })
            .ToList();

            return peritosVms;
        }        
    }
}
