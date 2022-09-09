using APIRest.Context;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.ViewModels;
using APIRest.Excepciones;
using System.Net;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace APIRest.Repositorios
{
    public class RepositorioUsuarios
    {
        private ExpertManagerContext _contexto;

        public RepositorioUsuarios(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public virtual async Task<Usuario> ObtenerPorId(int id)
        {            
            Usuario usuario = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .Where(usuario => usuario.Id == id)
                                             .FirstOrDefaultAsync();
            if (usuario is null)
                throw new CodigoErrorHttpException($"No existe el usuario con id {id}", HttpStatusCode.NotFound);

            return usuario;
        }        

        public string ObtenerHashContrasenia(string contrasenia)
        {
            byte[] salt = Encoding.ASCII.GetBytes("supercalifragilisticoespialidosomola_123");
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(contrasenia, salt, KeyDerivationPrf.HMACSHA256, 100000, 256 / 8));

            return hash;
        }

        public async Task<Usuario> ObtenerPorNombreYHashContrasenia(string nombre, string hashContrasenia)
        {
            Usuario usuario = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .FirstOrDefaultAsync(usuario => usuario.Nombre == nombre && usuario.Contrasenia == hashContrasenia);
            return usuario;
        }

        public virtual async Task<List<Usuario>> ObtenerTodos()
        {
            int idPermisoAdministracion = (int)TipoPermiso.Administracion;

            List<Usuario> usuarios = await _contexto.Usuarios
                                                    .Include(usuario => usuario.Permiso)
                                                    .Where(usuario => usuario.Permiso.Id == idPermisoAdministracion)
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

        public async Task<List<DetalleEstadisticaVm>> ObtenerEstadisticasPorIdUsuario(int id)
        {
            List<DetalleEstadisticaVm> estadisticasInicio = await _contexto.Siniestros
                                                                          .Include(siniestro => siniestro.UsuarioCreado)                                                                        
                                                                          .Include(siniestro => siniestro.Aseguradora)
                                                                          .Where(siniestro => siniestro.UsuarioCreado.Id == id)
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

        public bool EsValido(UsuarioVm usuarioVm)
        {
            if (usuarioVm.IdPermiso <= 0)
                throw new Exception("El permiso seleccionado no es válido");

            if (usuarioVm.Nombre is null || usuarioVm.Nombre.Length == 0)
                throw new Exception("El nombre está vacío");

            if (usuarioVm.HashContrasenia is null || usuarioVm.HashContrasenia.Length == 0)
                throw new Exception("La contraseña está vacía");

            return true;
        }
    }
}
