using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class MensajesController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public MensajesController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet("{idSiniestro}")]
        public async Task<List<MensajeVm>> ObtenerPorIdSiniestro(int idSiniestro)
        {
            List<Mensaje> mensajes = await _contexto.Mensajes
                                                    .Include(mensaje => mensaje.Siniestro)
                                                    .Include(mensaje => mensaje.Usuario)
                                                    .Where(mensaje => mensaje.Siniestro.Id == idSiniestro)
                                                    .ToListAsync();

            List<MensajeVm> mensajesVms = mensajes.Select(mensaje => new MensajeVm()
            {
                Id = mensaje.Id,
                Descripcion = mensaje.Descripcion,
                UsuarioCreado = mensaje.Usuario.Nombre
            })
            .ToList();

            return mensajesVms;
        }

        [HttpPost]
        public async Task<JsonResult> Crear(MensajeVm mensajeVm)
        {
            Usuario usuarioCreacion = await _contexto.Usuarios
                                                     .FirstOrDefaultAsync(usuario => usuario.Id == mensajeVm.IdUsuarioCreado);

            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == mensajeVm.IdSiniestro);

            Mensaje mensaje = new Mensaje()
            {
                Descripcion = mensajeVm.Descripcion,
                Usuario = usuarioCreacion,
                Siniestro = siniestro
            };

            try
            {
                _contexto.Add(mensaje);
                await _contexto.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }

        [HttpPost("RevisarCierre")]
        public async Task<JsonResult> CrearMensajeRevisarCierre(MensajeVm mensajeVm)
        {
            Usuario usuarioCreacion = await _contexto.Usuarios
                                                     .FirstOrDefaultAsync(usuario => usuario.Id == mensajeVm.IdUsuarioCreado);

            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == mensajeVm.IdSiniestro);

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

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
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
