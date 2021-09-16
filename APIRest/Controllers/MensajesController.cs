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
        public async Task<ActionResult> CrearMensajeRevisarCierre(MensajeVm mensajeVm)
        {
            Usuario usuario = await _repositorioUsuarios.ObtenerPorId(mensajeVm.IdUsuarioCreado);

            if (usuario is null)
                return NotFound($"No existe el usuario con id {mensajeVm.IdUsuarioCreado}");

            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(mensajeVm.IdSiniestro);

            if (siniestro is null)
                return NotFound($"No existe el siniestro con id {mensajeVm.IdSiniestro}");                                    

            Mensaje mensaje = new Mensaje()
            {
                Descripcion = "Revisar cierre",
                Usuario = usuario,
                Siniestro = siniestro
            };

            try
            {
                await _repositorioMensajes.Guardar(mensaje);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al crear el mensaje de revisar cierre");
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {            
            Mensaje mensaje = await _repositorioMensajes.ObtenerPorId(id);

            if (mensaje is null)
                return NotFound($"No existe el mensaje con id {id}");

            try
            {
                await _repositorioMensajes.Eliminar(mensaje);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al eliminar el mensaje");
            }            

            return Ok(true);            
        }
    }
}
