using APIRest.Context;
using APIRest.Excepciones;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace APIRest.Repositorios
{
    public class RepositorioPeritos : RepositorioUsuarios
    {
        private ExpertManagerContext _contexto;

        public RepositorioPeritos(ExpertManagerContext contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public override async Task<Usuario> ObtenerPorId(int id)
        {
            Usuario perito = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .Where(usuario => usuario.Id == id)
                                             .FirstOrDefaultAsync();            
            if (perito is null)
                throw new CodigoErrorHttpException($"No existe el perito con id {id}", HttpStatusCode.NotFound);

            return perito;
        }

        public override async Task<List<Usuario>> ObtenerTodos()
        {
            int idPermisoAdministracion = (int)TipoPermiso.Administracion;
            List<Usuario> peritos = await _contexto.Usuarios
                                                   .Include(usuario => usuario.Permiso)
                                                   .Where(usuario => usuario.Permiso.Id != idPermisoAdministracion)
                                                   .ToListAsync();
            if (peritos is null)
                throw new CodigoErrorHttpException("No existen peritos", HttpStatusCode.NotFound);

            return peritos;
        }

        public string ObtenerTextoEsPerito(int idPermiso)
        {
            string textoEsPerito;

            if (idPermiso == 1)
                textoEsPerito = "No";
            else
                textoEsPerito = "Sí";

            return textoEsPerito;
        }

        public async Task<int> ObtenerNumSiniestrosPorIdPerito(int id)
        {
            int numSiniestros = await _contexto.Siniestros
                                               .Include(siniestro => siniestro.Perito)
                                               .Where(siniestro => siniestro.Perito.Id == id)
                                               .CountAsync();
            return numSiniestros;
        }

        public async Task<List<DetalleEstadisticaVm>> ObtenerEstadisticasPorIdPerito(int id)
        {
            List<DetalleEstadisticaVm> estadisticasInicio = await _contexto.Siniestros
                                                                          .Include(siniestro => siniestro.Perito)
                                                                          .Include(siniestro => siniestro.Aseguradora)
                                                                          .Where(siniestro => siniestro.Perito.Id == id)
                                                                          .GroupBy(
                                                                              siniestro => siniestro.Aseguradora.Nombre,
                                                                              siniestro => siniestro.Id,
                                                                          (key, g) => new { Aseguradora = key, NumSiniestros = g.Count() })
                                                                          .Select(obj => new DetalleEstadisticaVm()
                                                                          {
                                                                              NombreAseguradora = obj.Aseguradora,
                                                                              NumSiniestros = obj.NumSiniestros
                                                                          })
                                                                          .ToListAsync();
            return estadisticasInicio;
        }

        public async Task<List<DetalleEstadisticaVm>> ObtenerSiniestrosCerrarPorIdPerito(int id)
        {
            int idEstadoValorado = (int)TipoEstado.Valorado;
            List<DetalleEstadisticaVm> numSiniestrosCerrar = await _contexto.Siniestros
                                                                           .Include(siniestro => siniestro.Perito)
                                                                           .Include(siniestro => siniestro.Estado)
                                                                           .Where(siniestro => siniestro.Perito.Id == id && siniestro.Estado.Id == idEstadoValorado)
                                                                           .GroupBy(
                                                                               siniestro => siniestro.Aseguradora.Nombre,
                                                                               siniestro => siniestro.Id,
                                                                           (key, g) => new { Aseguradora = key, NumSiniestros = g.Count() })
                                                                           .Select(obj => new DetalleEstadisticaVm()
                                                                           {
                                                                               NombreAseguradora = obj.Aseguradora,
                                                                               NumSiniestros = obj.NumSiniestros
                                                                           })
                                                                           .ToListAsync();
            return numSiniestrosCerrar;
        }
    }
}
