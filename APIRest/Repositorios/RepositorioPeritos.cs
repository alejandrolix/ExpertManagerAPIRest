using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioPeritos
    {
        private ExpertManagerContext _contexto;

        public RepositorioPeritos(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario> ObtenerPorId(int id)
        {
            int idPermPeritoResponsable = (int)TipoPermiso.PeritoResponsable;
            int idPermPeritoNoResponsable = (int)TipoPermiso.PeritoNoResponsable;            
            Usuario perito = await _contexto.Usuarios
                                            .Include(usuario => usuario.Permiso)
                                            .Where(usuario => usuario.Id == id && usuario.Permiso.Id >= idPermPeritoResponsable && usuario.Permiso.Id <= idPermPeritoNoResponsable)
                                            .FirstOrDefaultAsync();
            return perito;
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            int idPermisoAdministracion = (int)TipoPermiso.Administracion;
            List<Usuario> peritos = await _contexto.Usuarios
                                                   .Include(usuario => usuario.Permiso)
                                                   .Where(usuario => usuario.Permiso.Id != idPermisoAdministracion)
                                                   .ToListAsync();
            return peritos;
        }

        public async Task<int> ObtenerNumSiniestrosPorIdPerito(int id)
        {
            int numSiniestros = await _contexto.Siniestros
                                               .Include(siniestro => siniestro.Perito)
                                               .Where(siniestro => siniestro.Perito.Id == id)
                                               .CountAsync();
            return numSiniestros;
        }

        public async Task<List<EstadisticaInicioVm>> ObtenerEstadisticasPorIdPerito(int id)
        {
            List<EstadisticaInicioVm> estadisticasInicio = await _contexto.Siniestros
                                                                          .Include(siniestro => siniestro.Perito)
                                                                          .Include(siniestro => siniestro.Aseguradora)
                                                                          .Where(siniestro => siniestro.Perito.Id == id)
                                                                          .GroupBy(
                                                                              siniestro => siniestro.Aseguradora.Nombre,
                                                                              siniestro => siniestro.Id,
                                                                          (key, g) => new { Aseguradora = key, NumSiniestros = g.Count() })
                                                                          .Select(obj => new EstadisticaInicioVm()
                                                                          {
                                                                              NombreAseguradora = obj.Aseguradora,
                                                                              NumSiniestros = obj.NumSiniestros
                                                                          })
                                                                          .ToListAsync();
            return estadisticasInicio;
        }

        public async Task<List<EstadisticaInicioVm>> ObtenerSiniestrosCerrarPorIdPerito(int id)
        {
            int idEstadoValorado = (int)TipoEstado.Valorado;
            List<EstadisticaInicioVm> numSiniestrosCerrar = await _contexto.Siniestros
                                                                           .Include(siniestro => siniestro.Perito)
                                                                           .Include(siniestro => siniestro.Estado)
                                                                           .Where(siniestro => siniestro.Perito.Id == id && siniestro.Estado.Id == idEstadoValorado)
                                                                           .GroupBy(
                                                                               siniestro => siniestro.Aseguradora.Nombre,
                                                                               siniestro => siniestro.Id,
                                                                           (key, g) => new { Aseguradora = key, NumSiniestros = g.Count() })
                                                                           .Select(obj => new EstadisticaInicioVm()
                                                                           {
                                                                               NombreAseguradora = obj.Aseguradora,
                                                                               NumSiniestros = obj.NumSiniestros
                                                                           })
                                                                           .ToListAsync();
            return numSiniestrosCerrar;
        }
    }
}
