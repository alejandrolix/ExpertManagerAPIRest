using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
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
    public class InicioController : ControllerBase
    {
        private ExpertManagerContext _contexto;
        private RepositorioUsuarios _repositorioUsuarios;
        private RepositorioPeritos _repositorioPeritos;

        public InicioController(ExpertManagerContext contexto, RepositorioUsuarios repositorioUsuarios, RepositorioPeritos repositorioPeritos)
        {
            _contexto = contexto;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioPeritos = repositorioPeritos;
        }

        [HttpGet("{idUsuario}")]
        public async Task<ActionResult> ObtenerEstadisticas(int idUsuario)
        {
            Usuario usuario = await _repositorioUsuarios.ObtenerPorId(idUsuario);
            
            if (usuario is null)
                return NotFound($"No existe el usuario con id {idUsuario}");

            Usuario perito = await _repositorioPeritos.ObtenerPorId(idUsuario);

            if (perito is null && usuario is null)
                return NotFound($"No existe el perito con id {idUsuario}");
            
            int totalNumSiniestros = 0;
            List<EstadisticaInicioVm> numSiniestrosPorAseguradora = null;

            if (usuario != null)
            {
                totalNumSiniestros = await _repositorioUsuarios.ObtenerNumSiniestrosPorIdUsuario(idUsuario);
                numSiniestrosPorAseguradora = await _repositorioUsuarios.ObtenerEstadisticasPorIdUsuario(idUsuario);
            }

            if (perito != null)
            {
                totalNumSiniestros = await _repositorioPeritos.ObtenerNumSiniestrosPorIdPerito(idUsuario);
                numSiniestrosPorAseguradora = await _repositorioPeritos.ObtenerEstadisticasPorIdPerito(idUsuario);
            }                                    
                      
            EstadisticasVm estadisticasVm = new EstadisticasVm()
            {
                NumSiniestros = totalNumSiniestros,
                NumSiniestrosPorAseguradora = numSiniestrosPorAseguradora
            };

            // Comprobamos si el usuario a buscar es perito.
            Usuario perito1 = await _contexto.Usuarios
                                            .Include(usuario => usuario.Permiso)
                                            .FirstOrDefaultAsync(usuario => usuario.Id == idUsuario && usuario.Permiso.Id != 1);

            // El usuario es un perito
            if (perito1 != null)
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
