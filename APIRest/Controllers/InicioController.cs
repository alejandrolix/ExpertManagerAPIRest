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
    public class InicioController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public InicioController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet("{idUsuario}")]
        public async Task<ActionResult> ObtenerEstadisticas(int idUsuario)
        {
            Usuario usuario = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .FirstOrDefaultAsync(usuario => usuario.Id == idUsuario);
            
            if (usuario is null)
                return StatusCode(500, $"No existe el usuario con id {idUsuario}");

            int numSiniestros = await _contexto.Siniestros
                                               .Include(siniestro => siniestro.UsuarioCreado)
                                               .Include(siniestro => siniestro.Perito)
                                               .Where(siniestro => siniestro.UsuarioCreado.Id == idUsuario || siniestro.Perito.Id == idUsuario)
                                               .CountAsync();

            List<Tuple<string, int>> numSiniestrosPorAseguradora = await _contexto.Siniestros
                                                                                  .Include(siniestro => siniestro.UsuarioCreado)
                                                                                  .Include(siniestro => siniestro.Perito)
                                                                                  .Include(siniestro => siniestro.Aseguradora)
                                                                                  .Where(siniestro => siniestro.UsuarioCreado.Id == idUsuario || siniestro.Perito.Id == idUsuario)
                                                                                  .GroupBy(
                                                                                     siniestro => siniestro.Aseguradora.Nombre,
                                                                                     siniestro => siniestro.Id,
                                                                                   (key, g) => new { Aseguradora = key, NumSiniestros = g.Count() })
                                                                                  .Select(obj => new Tuple<string, int>(obj.Aseguradora, obj.NumSiniestros))
                                                                                  .ToListAsync();            
            EstadisticasVm estadisticasVm = new EstadisticasVm()
            {
                NumSiniestros = numSiniestros,
                NumSiniestrosPorAseguradora = numSiniestrosPorAseguradora
            };

            // Comprobamos si el usuario a buscar es perito.
            Usuario perito = await _contexto.Usuarios
                                            .Include(usuario => usuario.Permiso)
                                            .FirstOrDefaultAsync(usuario => usuario.Id == idUsuario && usuario.Permiso.Id != 1);

            // El usuario es un perito
            if (perito != null)
            {
                List<Tuple<string, int>> numSiniestrosCerrar = await _contexto.Siniestros
                                                                              .Include(siniestro => siniestro.UsuarioCreado)
                                                                              .Include(siniestro => siniestro.Perito)
                                                                              .Include(siniestro => siniestro.Estado)
                                                                              .Where(siniestro => (siniestro.UsuarioCreado.Id == idUsuario || siniestro.Perito.Id == idUsuario) &&
                                                                                     siniestro.Estado.Id == 3)
                                                                              .GroupBy(
                                                                                  siniestro => siniestro.Aseguradora.Nombre,
                                                                                  siniestro => siniestro.Id,
                                                                               (key, g) => new { Aseguradora = key, NumSiniestros = g.Count() })
                                                                               .Select(obj => new Tuple<string, int>(obj.Aseguradora, obj.NumSiniestros))
                                                                               .ToListAsync();

                estadisticasVm.NumSiniestrosCerrarPorAseguradora = numSiniestrosCerrar;
            }
            
            return Ok(estadisticasVm);
        }
    }
}
