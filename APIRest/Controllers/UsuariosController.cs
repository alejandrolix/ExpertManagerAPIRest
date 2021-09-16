using APIRest.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.ViewModels;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using APIRest.Repositorios;

namespace APIRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private ExpertManagerContext _contexto;
        private RepositorioUsuarios _repositorioUsuarios;
        private RepositorioPermisos _repositorioPermisos;

        public UsuariosController(ExpertManagerContext contexto, RepositorioUsuarios repositorioUsuarios, RepositorioPermisos repositorioPermisos)
        {
            _contexto = contexto;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioPermisos = repositorioPermisos;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerTodos()
        {
            List<Usuario> usuarios = await _repositorioUsuarios.ObtenerTodos();

            if (usuarios is null || usuarios.Count == 0)
                return NotFound("No existen usuarios");
            
            List<UsuarioVm> usuariosVms = usuarios.Select(usuario => new UsuarioVm()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,                
                IdPermiso = usuario.Permiso.Id,
                EsPerito = EsPerito(usuario.Permiso.Id),
                Permiso = usuario.Permiso.Nombre
            })
            .ToList();

            return Ok(usuariosVms);
        }

        private string EsPerito(int idPermiso)
        {
            string esPerito;

            if (idPermiso == 1)
                esPerito = "No";
            else
                esPerito = "Sí";

            return esPerito;
        }

        [HttpGet("{id}")]
        public async Task<UsuarioVm> ObtenerPorId(int id)
        {
            Usuario usuario = await _contexto.Usuarios
                                            .Include(usuario => usuario.Permiso)
                                            .OrderBy(usuario => usuario.Nombre)
                                            .FirstOrDefaultAsync(usuario => usuario.Id == id);

            UsuarioVm usuarioVm = new UsuarioVm()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                IdPermiso = usuario.Permiso.Id,
                Permiso = usuario.Permiso.Nombre,
                HashContrasenia = usuario.Contrasenia
            };

            if (EsPeritoNoResponsable(usuario.Permiso.Id))
                usuarioVm.ImpReparacionDanios = usuario.ImpRepacionDanios;

            return usuarioVm;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CrearUsuarioVm crearUsuarioVm)
        {
            Permiso permiso = await _repositorioPermisos.ObtenerPorId(crearUsuarioVm.IdPermiso);

            if (permiso is null)
                return NotFound($"No existe el permiso con id {crearUsuarioVm.IdPermiso}");

            Usuario usuario = new Usuario()
            {
                Nombre = crearUsuarioVm.Nombre,
                Contrasenia = crearUsuarioVm.HashContrasenia,
                Permiso = permiso
            };

            if (EsPeritoNoResponsable(permiso.Id))
                usuario.ImpRepacionDanios = crearUsuarioVm.ImpReparacionDanios;
            else
                usuario.ImpRepacionDanios = 0;

            try
            {
                await _repositorioUsuarios.Guardar(usuario);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al crear el usuario");
            }

            return Ok(true);
        }

        [HttpPost("IniciarSesion")]
        public async Task<RespuestaApi> IniciarSesion(UsuarioVm usuarioVm)
        {
            Usuario usuario = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .FirstOrDefaultAsync(usuario => usuario.Nombre == usuarioVm.Nombre &&
                                                                  usuario.Contrasenia == usuarioVm.HashContrasenia);
            int codigoRespuesta = 500;
            string mensaje = null;
            object datos = null;

            if (usuario is null)
                mensaje = $"No existe el usuario {usuarioVm.Nombre} o la contraseña es incorrecta";
            else
            {
                codigoRespuesta = 200;
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                UsuarioVm respuesta = new UsuarioVm()
                {
                    Nombre = usuario.Nombre,
                    Id = usuario.Id,
                    IdPermiso = usuario.Permiso.Id,
                    Token = token
                };

                datos = respuesta;
            }

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensaje,
                Datos = datos
            };

            return respuestaApi;            
        }

        private bool EsPeritoNoResponsable(int idPermiso)
        {
            return idPermiso == 3;
        }

        [HttpPut("{id}")]
        public async Task<JsonResult> Edit(int id, UsuarioVm usuarioVm)
        {
            try
            {
                Usuario usuario = await _contexto.Usuarios
                                                 .Include(usuario => usuario.Permiso)
                                                 .FirstOrDefaultAsync(usuario => usuario.Id == id);

                usuario.Nombre = usuarioVm.Nombre;
                usuario.Contrasenia = usuarioVm.HashContrasenia;              

                Permiso permiso = await _contexto.Permisos
                                                 .FirstOrDefaultAsync(permiso => permiso.Id == usuarioVm.IdPermiso);

                usuario.Permiso = permiso;

                if (EsPeritoNoResponsable(permiso.Id))        // Permiso Perito no responsable
                    usuario.ImpRepacionDanios = usuarioVm.ImpReparacionDanios;
                else
                    usuario.ImpRepacionDanios = 0;

                _contexto.Update(usuario);
                await _contexto.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                Usuario usuario = await _contexto.Usuarios
                                                 .FirstOrDefaultAsync(usuario => usuario.Id == id);
                if (usuario is null)
                    return new JsonResult(false);

                _contexto.Remove(usuario);
                await _contexto.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }
    }
}
