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
    public class SiniestrosController : ControllerBase
    {
        private ExpertManagerContext _contexto;

        public SiniestrosController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: SiniestrosController
        [HttpGet]
        public async Task<List<SiniestroVm>> Index()
        {
            List<Siniestro> siniestros = await _contexto.Siniestros
                                                        .Include(siniestro => siniestro.Aseguradora)
                                                        .Include(siniestro => siniestro.Estado)
                                                        .Include(siniestro => siniestro.UsuarioCreado)
                                                        .Include(siniestro => siniestro.Perito)        
                                                        .Include(siniestro => siniestro.Danio)
                                                        .OrderByDescending(siniestro => siniestro.FechaHoraAlta)
                                                        .ToListAsync();

            List<SiniestroVm> siniestrosVms = siniestros.Select(siniestro => new SiniestroVm()
            {
                Id = siniestro.Id,
                Estado = siniestro.Estado.Nombre,
                Aseguradora = siniestro.Aseguradora.Nombre,
                Descripcion = siniestro.Descripcion,
                Perito = siniestro.Perito.Nombre,
                FechaHoraAlta = siniestro.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm"),
                SujetoAfectado = siniestro.SujetoAfectado.ToString(),
                Danio = siniestro.Danio.Nombre,
                ImpValoracionDanios = $"{siniestro.ImpValoracionDanios.ToString("F")} €"
            })
            .ToList();

            return siniestrosVms;
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

        //// GET: SiniestrosController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: SiniestrosController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // DELETE: SiniestrosController/Delete/5
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
