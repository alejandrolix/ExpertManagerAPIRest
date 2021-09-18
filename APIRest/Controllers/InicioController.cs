using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        private RepositorioUsuarios _repositorioUsuarios;
        private RepositorioPeritos _repositorioPeritos;
        private RepositorioPermisos _repositorioPermisos;

        public InicioController(RepositorioUsuarios repositorioUsuarios, RepositorioPeritos repositorioPeritos, RepositorioPermisos repositorioPermisos)
        {
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioPeritos = repositorioPeritos;
            _repositorioPermisos = repositorioPermisos;
        }

        [HttpGet("{idUsuario}")]
        public async Task<ActionResult> ObtenerEstadisticas(int idUsuario)
        {
            Usuario usuario = await _repositorioUsuarios.ObtenerPorId(idUsuario);                                   

            if (usuario is null)                                       
                return NotFound($"No existe el usuario con id {idUsuario}");                            

            int totalNumSiniestros;
            List<EstadisticaInicioVm> numSiniestrosPorAseguradora;
            List<EstadisticaInicioVm> siniestrosCerrarPorAseguradora = null;
            bool tienePermisoAdministracion = _repositorioPermisos.TienePermisoAdministracion(usuario.Permiso.Id);

            if (tienePermisoAdministracion)
            {
                totalNumSiniestros = await _repositorioUsuarios.ObtenerNumSiniestrosPorIdUsuario(idUsuario);
                numSiniestrosPorAseguradora = await _repositorioUsuarios.ObtenerEstadisticasPorIdUsuario(idUsuario);
            }
            else
            {
                totalNumSiniestros = await _repositorioPeritos.ObtenerNumSiniestrosPorIdPerito(idUsuario);
                numSiniestrosPorAseguradora = await _repositorioPeritos.ObtenerEstadisticasPorIdPerito(idUsuario);
                siniestrosCerrarPorAseguradora = await _repositorioPeritos.ObtenerSiniestrosCerrarPorIdPerito(idUsuario);
            }                                               
                      
            EstadisticasVm estadisticasVm = new EstadisticasVm()
            {
                NumSiniestros = totalNumSiniestros,
                NumSiniestrosPorAseguradora = numSiniestrosPorAseguradora,
                NumSiniestrosCerrarPorAseguradora = siniestrosCerrarPorAseguradora
            };                        
            
            return Ok(estadisticasVm);
        }
    }
}
