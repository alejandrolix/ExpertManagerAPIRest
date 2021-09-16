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
    public class MensajesController : ControllerBase
    {
        private ExpertManagerContext _contexto;
        private RepositorioMensajes _repositorioMensajes;
        private RepositorioUsuarios _repositorioUsuarios;
        private RepositorioSiniestros _repositorioSiniestros;

        public MensajesController(ExpertManagerContext contexto, RepositorioMensajes repositorioMensajes, RepositorioUsuarios repositorioUsuarios, RepositorioSiniestros repositorioSiniestros)
        {
            _contexto = contexto;
            _repositorioMensajes = repositorioMensajes;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioSiniestros = repositorioSiniestros;
        }

        [HttpGet("{idSiniestro}")]
        public async Task<ActionResult> ObtenerPorIdSiniestro(int idSiniestro)
        {
            List<Mensaje> mensajes = await _repositorioMensajes.ObtenerTodosPorIdSiniestro(idSiniestro);

            if (mensajes is null || mensajes.Count == 0)
                return NotFound("No existen mensajes");

            List<MensajeVm> mensajesVms = mensajes.Select(mensaje => new MensajeVm()
            {
                Id = mensaje.Id,
                Descripcion = mensaje.Descripcion,
                UsuarioCreado = mensaje.Usuario.Nombre
            })
            .ToList();

            return Ok(mensajesVms);
        }

        [HttpPost]
        public async Task<ActionResult> Crear(MensajeVm mensajeVm)
        {
            Usuario usuario = await _repositorioUsuarios.ObtenerPorId(mensajeVm.IdUsuarioCreado);

            if (usuario is null)
                return NotFound($"No existe el usuario con id {mensajeVm.IdUsuarioCreado}");

            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(mensajeVm.IdSiniestro);

            if (siniestro is null)            
                return NotFound($"No existe el siniestro con id {mensajeVm.IdSiniestro}");                                    
            
            Mensaje mensaje = new Mensaje()
            {
                Descripcion = mensajeVm.Descripcion,
                Usuario = usuario,
                Siniestro = siniestro
            };

            try
            {
                await _repositorioMensajes.Guardar(mensaje);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al crear el mensaje");
            }        
                        
            return Ok(true);            
        }

        [HttpPost("RevisarCierre")]
        public async Task<RespuestaApi> CrearMensajeRevisarCierre(MensajeVm mensajeVm)
        {
            Usuario usuarioCreacion = await _contexto.Usuarios
                                                     .FirstOrDefaultAsync(usuario => usuario.Id == mensajeVm.IdUsuarioCreado);

            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == mensajeVm.IdSiniestro);            
            int codigoRespuesta = 500;
            string mensajeError = null;
            object datos = false;

            if (usuarioCreacion is null)
                mensajeError = $"No existe el usuario con id {mensajeVm.IdUsuarioCreado}";
            else if (siniestro is null)
                mensajeError = $"No existe el siniestro con id {mensajeVm.IdSiniestro}";

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensajeError,
                Datos = datos
            };

            if (!string.IsNullOrEmpty(mensajeError))
                return respuestaApi;

            Mensaje mensaje = new Mensaje()
            {
                Descripcion = "Revisar cierre",
                Usuario = usuarioCreacion,
                Siniestro = siniestro
            };

            try
            {
                _contexto.Add(mensaje);
                await _contexto.SaveChangesAsync();

                codigoRespuesta = 200;
                datos = true;
            }
            catch (Exception ex)
            {
                datos = false;
            }

            return respuestaApi;
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Eliminar(int id)
        {
            try
            {
                Mensaje mensaje = await _contexto.Mensajes
                                                 .FirstOrDefaultAsync(mensaje => mensaje.Id == id);
                if (mensaje is null)
                    return new JsonResult(false);

                _contexto.Remove(mensaje);
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
