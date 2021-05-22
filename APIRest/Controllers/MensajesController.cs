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
    }
}
