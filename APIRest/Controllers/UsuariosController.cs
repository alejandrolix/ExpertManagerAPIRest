using APIRest.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.ViewModels;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;

namespace APIRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public UsuariosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<List<UsuarioVm>> ObtenerTodos()
        {
            List<Usuario> usuarios = await _contexto.Usuarios
                                                            .Include(usuario => usuario.Permiso)
                                                            .OrderBy(usuario => usuario.Nombre)
                                                            .ToListAsync();

            List<UsuarioVm> usuariosVms = usuarios.Select(usuario => new UsuarioVm()
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,                
                IdPermiso = usuario.Permiso.Id,
                EsPerito = EsPerito(usuario.Permiso.Id),
                Permiso = usuario.Permiso.Nombre
            }).ToList();

            return usuariosVms;
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
        public async Task<JsonResult> Create(CrearUsuarioVm crearUsuarioVm)
        {
            try
            {                                
                Permiso permiso = await _contexto.Permisos
                                                 .FirstOrDefaultAsync(permiso => permiso.Id == crearUsuarioVm.IdPermiso);

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

                _contexto.Add(usuario);
                await _contexto.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }

        [HttpPost("IniciarSesion")]
        public async Task<JsonResult> IniciarSesion(UsuarioVm usuarioVm)
        {
            Usuario usuario = await _contexto.Usuarios
                                             .Include(usuario => usuario.Permiso)
                                             .FirstOrDefaultAsync(usuario => usuario.Nombre == usuarioVm.Nombre &&
                                                                  usuario.Contrasenia == usuarioVm.HashContrasenia);
            if (usuario is null)
                return new JsonResult(false);
            else
            {
                UsuarioVm respuesta = new UsuarioVm()
                {
                    Nombre = usuario.Nombre,
                    IdPermiso = usuario.Permiso.Id
                };

                return new JsonResult(respuesta);
            }
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
