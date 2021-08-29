using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
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
    public class PeritosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public PeritosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: PeritosController
        [HttpGet]
        public async Task<RespuestaApi> Index()
        {
            List<Usuario> peritos = await _contexto.Usuarios
                                                   .Include(usuario => usuario.Permiso)
                                                   .Where(usuario => usuario.Permiso.Id != 1)
                                                   .ToListAsync();
            int codigoRespuesta;
            string mensaje = null;

            if (peritos == null || peritos.Count == 0)
            {
                codigoRespuesta = 500;
                mensaje = "No existen peritos";
            }
            else
            {
                codigoRespuesta = 200;

                List<PeritoVm> peritosVms = peritos.Select(perito => new PeritoVm()
                {
                    Id = perito.Id,
                    Nombre = perito.Nombre
                })
                .ToList();
            }

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensaje,
                Datos = peritos
            };

            return respuestaApi;
        }

        [HttpGet("ImporteReparacionDanios/{idPerito}")]
        public async Task<RespuestaApi> ObtenerImpReparacionDaniosPorIdPerito(int idPerito)
        {
            Usuario perito = await _contexto.Usuarios
                                            .Include(usuario => usuario.Permiso)
                                            .FirstOrDefaultAsync(usuario => usuario.Permiso.Id != 1 && usuario.Id == idPerito);
            int codigoRespuesta = 500;
            string mensaje = null;
            object datos = null;

            if (perito == null)
                mensaje = $"No existe importe de reparación de daños del perito con id {idPerito}";
            else if (perito.ImpRepacionDanios == 0)
                mensaje = $"El importe de reparación de daños del perito con id {idPerito} es cero";
            else
            {
                codigoRespuesta = 200;
                datos = perito.ImpRepacionDanios;
            }

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensaje,
                Datos = datos
            };

            return respuestaApi;
        }
    }
}
