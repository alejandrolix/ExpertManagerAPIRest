using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
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

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Usuario> peritos = await _contexto.Usuarios
                                                   .Include(usuario => usuario.Permiso)
                                                   .Where(usuario => usuario.Permiso.Id != 1)
                                                   .ToListAsync();     
            
            if (peritos is null || peritos.Count == 0)                            
                return StatusCode(500, "No existen peritos");            
            
            List<PeritoVm> peritosVms = peritos.Select(perito => new PeritoVm()
            {
                Id = perito.Id,
                Nombre = perito.Nombre
            })
            .ToList();            

            return Ok(peritosVms);
        }

        [HttpGet("ImporteReparacionDanios/{idPerito}")]
        public async Task<ActionResult> ObtenerImpReparacionDaniosPorIdPerito(int idPerito)
        {
            Usuario perito = await _contexto.Usuarios
                                            .Include(usuario => usuario.Permiso)
                                            .FirstOrDefaultAsync(usuario => usuario.Permiso.Id != 1 && usuario.Id == idPerito);
            string mensaje = null;

            if (perito is null)
                mensaje = $"No existe importe de reparación de daños del perito con id {idPerito}";
            else if (perito.ImpRepacionDanios == 0)
                mensaje = $"El importe de reparación de daños del perito con id {idPerito} es cero";

            if (mensaje != null)            
                return StatusCode(500, mensaje);            

            return Ok(perito.ImpRepacionDanios);
        }
    }
}
