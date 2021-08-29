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
        public async Task<RespuestaApi> Index(int idPerito, int idAseguradora)
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
                siniestros = await _contexto.Siniestros
                                            .Include(siniestro => siniestro.Aseguradora)
                                            .Include(siniestro => siniestro.Estado)
                                            .Include(siniestro => siniestro.UsuarioCreado)
                                            .Include(siniestro => siniestro.Perito)
                                            .Include(siniestro => siniestro.Danio)
                                            .Where(siniestro => siniestro.Perito.Id == idPerito)
                                            .ToListAsync();

            if (idAseguradora != 0)
                siniestros = siniestros.Where(siniestro => siniestro.Aseguradora.Id == idAseguradora)
                                        .ToList();

            siniestros = siniestros.OrderByDescending(siniestro => siniestro.FechaHoraAlta)
                                   .ToList();

            int codigoRespuesta;
            string mensaje = null;
            object datos = null;

            if (siniestros == null || siniestros.Count == 0)
            {
                codigoRespuesta = 500;
                mensaje = "No existen siniestros";
            }
            else
            {
                codigoRespuesta = 200;

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

                datos = siniestrosVms;
            }

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensaje,
                Datos = datos
            };

            return respuestaApi;
        }

        [HttpGet("PeritoNoResponsable")]
        public async Task<RespuestaApi> ObtenerPorPeritoNoResponsable(int idPerito, int idAseguradora)
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

            int codigoRespuesta;
            string mensaje = null;
            object datos = null;

            if (siniestros == null || siniestros.Count == 0)
            {
                codigoRespuesta = 500;
                mensaje = "No existen siniestros";
            }
            else
            {
                codigoRespuesta = 200;

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

                datos = siniestrosVms;
            }

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensaje,
                Datos = datos
            };            

            return respuestaApi;
        }

        [HttpGet("PeritoResponsable")]
        public async Task<RespuestaApi> ObtenerPorPeritoResponsable(int idPerito, int idAseguradora)
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

            int codigoRespuesta;
            string mensaje = null;
            object datos = null;

            if (siniestros == null || siniestros.Count == 0)
            {
                codigoRespuesta = 500;
                mensaje = "No existen siniestros";
            }
            else
            {
                codigoRespuesta = 200;

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

                datos = siniestrosVms;
            }

            RespuestaApi respuestaApi = new RespuestaApi
            {
                CodigoRespuesta = codigoRespuesta,
                Mensaje = mensaje,
                Datos = datos
            };
            
            return respuestaApi;
        }

        private List<Siniestro> ObtenerSiniestrosPorIdAseguradora(int idAseguradora, List<Siniestro> siniestros)
        {
            return siniestros.Where(siniestro => siniestro.Aseguradora.Id == idAseguradora)
                             .ToList();
        }

        [HttpPut("Cerrar/{id}")]
        public async Task<RespuestaApi> Cerrar(int id)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == id);

            Estado estadoCerrado = await _contexto.Estados
                                                  .FirstOrDefaultAsync(estado => estado.Id == 4);
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
                mensaje = $"No existe el siniestro con id {id}";
                return respuestaApi;
            }
                
            try
            {
                siniestro.Estado = estadoCerrado;

                _contexto.Update(siniestro);
                await _contexto.SaveChangesAsync();

                codigoRespuesta = 200;
                datos = true;
            }
            catch (Exception ex)
            {
                mensaje = $"No se ha podido cerrar el siniestro con id {id}";             
            }            

            return respuestaApi;
        }

        [HttpGet("{id}")]
        public async Task<SiniestroVm> ObtenerPorId(int id)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                .Include(siniestro => siniestro.Aseguradora)
                                                .Include(siniestro => siniestro.Estado)
                                                .Include(siniestro => siniestro.UsuarioCreado)
                                                .Include(siniestro => siniestro.Perito)
                                                .Include(siniestro => siniestro.Danio)
                                                .FirstOrDefaultAsync(siniestro => siniestro.Id == id);

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

            return siniestroVm;
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
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                Siniestro siniestro = await _contexto.Siniestros
                                                     .FirstOrDefaultAsync(siniestro => siniestro.Id == id);
                if (siniestro is null)
                    return new JsonResult(false);

                _contexto.Remove(siniestro);
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
