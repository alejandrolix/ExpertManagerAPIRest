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
        private RepositorioPeritos _repositorioPeritos;

        public UsuariosController(ExpertManagerContext contexto, RepositorioUsuarios repositorioUsuarios, RepositorioPermisos repositorioPermisos, RepositorioPeritos repositorioPeritos)
        {
            _contexto = contexto;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioPermisos = repositorioPermisos;
            _repositorioPeritos = repositorioPeritos;
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
                EsPerito = _repositorioPeritos.ObtenerTextoEsPerito(usuario.Permiso.Id),
                Permiso = usuario.Permiso.Nombre
            })
            .ToList();

            return Ok(usuariosVms);
        }        

        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            Usuario usuario = await _repositorioUsuarios.ObtenerPorId(id);

            if (usuario is null)
                return NotFound($"No existe el usuario con id {id}");

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

            return Ok(usuarioVm);
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
        public async Task<ActionResult> IniciarSesion(UsuarioVm usuarioVm)
        {
            Usuario usuario = await _repositorioUsuarios.ObtenerPorNombreYHashContrasenia(usuarioVm.Nombre, usuarioVm.HashContrasenia);

            if (usuario is null)
                return NotFound($"No existe el usuario {usuarioVm.Nombre} o la contraseña es incorrecta");
                            
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            UsuarioVm respuesta = new UsuarioVm()
            {
                Nombre = usuario.Nombre,
                Id = usuario.Id,
                IdPermiso = usuario.Permiso.Id,
                Token = token
            };

            return Ok(respuesta);
        }        

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, UsuarioVm usuarioVm)
        {
            Usuario usuario = await _repositorioUsuarios.ObtenerPorId(id);
            Usuario perito = await _repositorioPeritos.ObtenerPorId(id);

            if (usuario is null && perito is null)
                return NotFound($"No existe el usuario o perito con id {id}");

            usuario = perito;
            usuario.Nombre = usuarioVm.Nombre;
            usuario.Contrasenia = usuarioVm.HashContrasenia;

            Permiso permiso = await _repositorioPermisos.ObtenerPorId(usuarioVm.IdPermiso);

            if (permiso is null)
                return NotFound($"No existe el permiso con id {usuarioVm.IdPermiso}");

            usuario.Permiso = permiso;

            if (EsPeritoNoResponsable(permiso.Id))        // Permiso Perito no responsable
                usuario.ImpRepacionDanios = usuarioVm.ImpReparacionDanios;
            else
                usuario.ImpRepacionDanios = 0;

            try
            {
                await _repositorioUsuarios.Actualizar(usuario);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al editar el usuario");
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {            
            Usuario usuario = await _repositorioUsuarios.ObtenerPorId(id);
            Usuario perito = await _repositorioPeritos.ObtenerPorId(id);

            if (usuario is null && perito is null)
                return NotFound($"No existe el usuario o perito con id {id}");

            usuario = perito;

            try
            {
                await _repositorioUsuarios.Eliminar(usuario);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al eliminar el usuario o el perito");
            }

            return Ok(true);
        }
    }
}
