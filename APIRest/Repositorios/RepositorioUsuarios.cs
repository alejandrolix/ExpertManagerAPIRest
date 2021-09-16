using APIRest.Context;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.ViewModels;

namespace APIRest.Repositorios
{
    public class RepositorioUsuarios
    {
        private ExpertManagerContext _contexto;

        public RepositorioUsuarios(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Usuario> ObtenerPorId(int id)
        {
            int idPermisoAdministracion = (int)TipoPermiso.Administracion;
            Usuario usuario = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .Where(usuario => usuario.Id == id && usuario.Permiso.Id == idPermisoAdministracion)
                                             .FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            List<Usuario> usuarios = await _contexto.Usuarios
                                                    .Include(usuario => usuario.Permiso)
                                                    .OrderBy(usuario => usuario.Nombre)
                                                    .ToListAsync();
            return usuarios;
        }

        public async Task<int> ObtenerNumSiniestrosPorIdUsuario(int id)
        {
            int numSiniestros = await _contexto.Siniestros
                                               .Include(siniestro => siniestro.UsuarioCreado)                                               
                                               .Where(siniestro => siniestro.UsuarioCreado.Id == id)
                                               .CountAsync();
            return numSiniestros;
        }

        public async Task<List<EstadisticaInicioVm>> ObtenerEstadisticasPorIdUsuario(int id)
        {
            List<EstadisticaInicioVm> estadisticasInicio = await _contexto.Siniestros
                                                                          .Include(siniestro => siniestro.UsuarioCreado)                                                                        
                                                                          .Include(siniestro => siniestro.Aseguradora)
                                                                          .Where(siniestro => siniestro.UsuarioCreado.Id == id)
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

        public async Task Guardar(Usuario usuario)
        {
            try
            {
                _contexto.Add(usuario);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Actualizar(Usuario usuario)
        {
            try
            {
                _contexto.Update(usuario);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Eliminar(Usuario usuario)
        {
            try
            {
                _contexto.Remove(usuario);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
