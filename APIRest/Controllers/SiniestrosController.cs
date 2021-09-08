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
    public class SiniestrosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public SiniestrosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: SiniestrosController
        [HttpGet]
        public async Task<ActionResult> Index(int idPerito, int idAseguradora)
        {
            List<Siniestro> siniestros = null;

            if (idPerito == 0)
                siniestros = await _contexto.Siniestros
                                            .Include(siniestro => siniestro.Aseguradora)
                                            .Include(siniestro => siniestro.Estado)
                                            .Include(siniestro => siniestro.UsuarioCreado)
                                            .Include(siniestro => siniestro.Perito)
                                            .Include(siniestro => siniestro.Danio)
                                            .ToListAsync();
            else
                siniestros = await ObtenerPorIdPerito(idPerito);

            if (idAseguradora != 0)
                siniestros = siniestros.Where(siniestro => siniestro.Aseguradora.Id == idAseguradora)
                                        .ToList();            

            if (siniestros.Count == 0)            
                return StatusCode(500, "No existen siniestros");            

            siniestros = siniestros.OrderByDescending(siniestro => siniestro.FechaHoraAlta)
                                   .ToList();

            List<SiniestroVm> siniestrosVms = siniestros.Select(siniestro => new SiniestroVm()
            {
                Id = siniestro.Id,
                IdEstado = siniestro.Estado.Id,
                Estado = siniestro.Estado.Nombre,
                Aseguradora = siniestro.Aseguradora.Nombre,
                Descripcion = siniestro.Descripcion,
                Perito = siniestro.Perito.Nombre,
                FechaHoraAlta = siniestro.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm"),
                SujetoAfectado = siniestro.SujetoAfectado.ToString(),
                Danio = siniestro.Danio.Nombre,
                ImpValoracionDanios = $"{siniestro.ImpValoracionDanios:F} €"
            })
            .ToList();

            return Ok(siniestrosVms);            
        }

        private async Task<List<Siniestro>> ObtenerPorIdPerito(int idPerito)
        {
            List<Siniestro> siniestros = await _contexto.Siniestros
                                            .Include(siniestro => siniestro.Aseguradora)
                                            .Include(siniestro => siniestro.Estado)
                                            .Include(siniestro => siniestro.UsuarioCreado)
                                            .Include(siniestro => siniestro.Perito)
                                            .Include(siniestro => siniestro.Danio)
                                            .Where(siniestro => siniestro.Perito.Id == idPerito)
                                            .ToListAsync();
            return siniestros;
        }

        [HttpGet("PeritoNoResponsable")]
        public async Task<ActionResult> ObtenerPorPeritoNoResponsable(int idPerito, int idAseguradora)
        {
            List<Siniestro> siniestros = await _contexto.Siniestros
                                                        .Include(siniestro => siniestro.Aseguradora)
                                                        .Include(siniestro => siniestro.Estado)
                                                        .Include(siniestro => siniestro.UsuarioCreado)
                                                        .Include(siniestro => siniestro.Perito)
                                                        .Include(siniestro => siniestro.Danio)
                                                        .Where(siniestro => siniestro.Perito.Id == idPerito)
                                                        .ToListAsync();
            if (idAseguradora != 0)
                siniestros = ObtenerSiniestrosPorIdAseguradora(idAseguradora, siniestros);

            siniestros = siniestros.OrderByDescending(siniestro => siniestro.FechaHoraAlta)
                                   .ToList();                        

            if (siniestros is null || siniestros.Count == 0)                            
                return StatusCode(500, "No existen siniestros");            

            List<SiniestroVm> siniestrosVms = siniestros.Select(siniestro => new SiniestroVm()
            {
                Id = siniestro.Id,
                IdEstado = siniestro.Estado.Id,
                Estado = siniestro.Estado.Nombre,
                Aseguradora = siniestro.Aseguradora.Nombre,
                Descripcion = siniestro.Descripcion,
                Perito = siniestro.Perito.Nombre,
                FechaHoraAlta = siniestro.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm"),
                SujetoAfectado = siniestro.SujetoAfectado.ToString(),
                Danio = siniestro.Danio.Nombre,
                ImpValoracionDanios = $"{siniestro.ImpValoracionDanios:F} €"
            })
            .ToList();

            return Ok(siniestrosVms);
        }

        [HttpGet("PeritoResponsable")]
        public async Task<ActionResult> ObtenerPorPeritoResponsable(int idPerito, int idAseguradora)
        {
            List<Siniestro> siniestros = await _contexto.Siniestros
                                                        .Include(siniestro => siniestro.Aseguradora)
                                                        .Include(siniestro => siniestro.Estado)
                                                        .Include(siniestro => siniestro.UsuarioCreado)
                                                        .Include(siniestro => siniestro.Perito)
                                                        .Include(siniestro => siniestro.Perito.Permiso)
                                                        .Include(siniestro => siniestro.Danio)
                                                        .Where(siniestro => siniestro.Perito.Id == idPerito || siniestro.Perito.Permiso.Id == 3)
                                                        .ToListAsync();
            if (idAseguradora != 0)
                siniestros = ObtenerSiniestrosPorIdAseguradora(idAseguradora, siniestros);

            siniestros = siniestros.OrderByDescending(siniestro => siniestro.FechaHoraAlta)
                                   .ToList();                       

            if (siniestros is null || siniestros.Count == 0)                            
                return StatusCode(500, "No existen siniestros");            
                   
            List<SiniestroVm> siniestrosVms = siniestros.Select(siniestro => new SiniestroVm()
            {
                Id = siniestro.Id,
                IdEstado = siniestro.Estado.Id,
                Estado = siniestro.Estado.Nombre,
                Aseguradora = siniestro.Aseguradora.Nombre,
                Descripcion = siniestro.Descripcion,
                Perito = siniestro.Perito.Nombre,
                FechaHoraAlta = siniestro.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm"),
                SujetoAfectado = siniestro.SujetoAfectado.ToString(),
                Danio = siniestro.Danio.Nombre,
                ImpValoracionDanios = $"{siniestro.ImpValoracionDanios:F} €"
            })
            .ToList();                            

            return Ok(siniestrosVms);
        }

        private List<Siniestro> ObtenerSiniestrosPorIdAseguradora(int idAseguradora, List<Siniestro> siniestros)
        {
            return siniestros.Where(siniestro => siniestro.Aseguradora.Id == idAseguradora)
                             .ToList();
        }

        [HttpPut("Cerrar/{id}")]
        public async Task<ActionResult> Cerrar(int id)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == id);

            Estado estadoCerrado = await _contexto.Estados
                                                  .FirstOrDefaultAsync(estado => estado.Id == 4);
            string mensajeError = null;
            bool estaCerrado;            

            if (siniestro is null)
            {
                mensajeError = $"No existe el siniestro con id {id}";
                return NotFound(mensajeError);            
            }                     
                
            try
            {
                siniestro.Estado = estadoCerrado;

                _contexto.Update(siniestro);
                await _contexto.SaveChangesAsync();

                estaCerrado = true;
            }
            catch (Exception ex)
            {
                mensajeError = $"No se ha podido cerrar el siniestro con id {id}";
                estaCerrado = false;
            }

            if (estaCerrado)
                return Ok(estaCerrado);

            return StatusCode(500, mensajeError);
        }

        [HttpGet("{id}")]
        public async Task<RespuestaApi> ObtenerPorId(int id)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                 .Include(siniestro => siniestro.Aseguradora)
                                                 .Include(siniestro => siniestro.Estado)
                                                 .Include(siniestro => siniestro.UsuarioCreado)
                                                 .Include(siniestro => siniestro.Perito)
                                                 .Include(siniestro => siniestro.Danio)
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == id);
            int codigoRespuesta = 500;
            string mensaje = null;
            object datos = false;

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensaje,
                Datos = datos
            };

            if (siniestro is null)
            {
                mensaje = $"No se ha encontrado el siniestro con id {id}";
                respuestaApi.Mensaje = mensaje;

                return respuestaApi;
            }

            codigoRespuesta = 200;
            SiniestroVm siniestroVm = new SiniestroVm()
            {
                Id = siniestro.Id,
                IdEstado = siniestro.EstadoId,
                Estado = siniestro.Estado.Nombre,
                IdAseguradora = siniestro.AseguradoraId,
                Aseguradora = siniestro.Aseguradora.Nombre,
                Direccion = siniestro.Direccion,
                Descripcion = siniestro.Descripcion,
                IdPerito = siniestro.PeritoId,
                Perito = siniestro.Perito.Nombre,
                FechaHoraAlta = siniestro.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm"),
                IdSujetoAfectado = (int) siniestro.SujetoAfectado,
                SujetoAfectado = siniestro.SujetoAfectado.ToString(),
                IdDanio = siniestro.DanioId.Value,
                Danio = siniestro.Danio.Nombre,
                ImpValoracionDanios = $"{siniestro.ImpValoracionDanios.ToString("F")} €"
            };
            datos = siniestroVm;

            respuestaApi.CodigoRespuesta = codigoRespuesta;
            respuestaApi.Datos = datos;

            return respuestaApi;
        }

        // POST: SiniestrosController/Create
        [HttpPost]        
        public async Task<JsonResult> Create(CrearSiniestroVm crearSiniestroVm)
        {
            try
            {
                Estado estado = await _contexto.Estados
                                                   .Where(estado => estado.Id == 2)
                                                   .FirstOrDefaultAsync();

                Aseguradora aseguradora = await _contexto.Aseguradoras
                                                         .Where(aseguradora => aseguradora.Id == crearSiniestroVm.IdAseguradora)
                                                         .FirstOrDefaultAsync();

                Usuario usuarioCreado = await _contexto.Usuarios
                                                        .Where(usuario => usuario.Id == crearSiniestroVm.IdUsuarioAlta)
                                                        .FirstOrDefaultAsync();

                SujetoAfectado sujetoAfectado = (SujetoAfectado)crearSiniestroVm.IdSujetoAfectado;

                Usuario perito = await _contexto.Usuarios
                                                .Where(usuario => usuario.Id == crearSiniestroVm.IdPerito)
                                                .FirstOrDefaultAsync();

                Danio danio = await _contexto.Danios
                                            .Where(danio => danio.Id == crearSiniestroVm.IdDanio)
                                            .FirstOrDefaultAsync();

                Siniestro siniestro = new Siniestro()
                {
                    Estado = estado,
                    Aseguradora = aseguradora,
                    Direccion = crearSiniestroVm.Direccion,
                    Descripcion = crearSiniestroVm.Descripcion,
                    UsuarioCreado = usuarioCreado,
                    FechaHoraAlta = DateTime.Now,
                    SujetoAfectado = sujetoAfectado,
                    ImpValoracionDanios = 0.00M,
                    Perito = perito,
                    Danio = danio
                };

                _contexto.Add(siniestro);
                int numRegistros = await _contexto.SaveChangesAsync();

                if (numRegistros != 0)
                    return new JsonResult(true);                
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }

            return new JsonResult(false);
        }

        [HttpPut("{id}")]        
        public async Task<JsonResult> Edit(int id, SiniestroVm siniestroVm)
        {
            try
            {
                Estado estado = await _contexto.Estados
                                                .Where(estado => estado.Id == siniestroVm.IdEstado)
                                                .FirstOrDefaultAsync();

                Aseguradora aseguradora = await _contexto.Aseguradoras
                                                         .Where(aseguradora => aseguradora.Id == siniestroVm.IdAseguradora)
                                                         .FirstOrDefaultAsync();                

                SujetoAfectado sujetoAfectado = (SujetoAfectado) siniestroVm.IdSujetoAfectado;

                Usuario perito = await _contexto.Usuarios
                                                .Where(usuario => usuario.Id == siniestroVm.IdPerito)
                                                .FirstOrDefaultAsync();

                Danio danio = await _contexto.Danios
                                            .Where(danio => danio.Id == siniestroVm.IdDanio)
                                            .FirstOrDefaultAsync();

                Siniestro siniestro = await _contexto.Siniestros
                                                    .Include(siniestro => siniestro.Estado)
                                                    .Include(siniestro => siniestro.Aseguradora)
                                                    .Include(siniestro => siniestro.Perito)
                                                    .Include(siniestro => siniestro.Danio)
                                                    .Include(siniestro => siniestro.UsuarioCreado)
                                                    .Where(siniestro => siniestro.Id == id)
                                                    .FirstOrDefaultAsync();

                siniestro.Estado = estado;
                siniestro.Aseguradora = aseguradora;
                siniestro.Direccion = siniestroVm.Direccion;
                siniestro.Descripcion = siniestroVm.Descripcion;
                siniestro.SujetoAfectado = sujetoAfectado;
                siniestro.Perito = perito;
                siniestro.Danio = danio;

                if (siniestroVm.IdEstado == 3)      // Estado Valorado
                    siniestro.ImpValoracionDanios = decimal.Parse(siniestroVm.ImpValoracionDanios);
                else
                    siniestro.ImpValoracionDanios = 0;

                _contexto.Update(siniestro);
                await _contexto.SaveChangesAsync();
                
                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }            
        }

        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(int id)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == id);            
            string mensaje = null;
            bool estaEliminado;            

            if (siniestro is null)
            {
                mensaje = $"No existe el siniestro con id {id}";
                return NotFound(mensaje);
            }

            try
            {
                _contexto.Remove(siniestro);
                await _contexto.SaveChangesAsync();

                estaEliminado = true;
            }
            catch (Exception ex)
            {
                mensaje = $"No se ha podido eliminar el siniestro con id {id}";
                estaEliminado = false;
            }

            if (estaEliminado)
                return Ok(estaEliminado);

            return StatusCode(500, mensaje);
        }
    }
}
