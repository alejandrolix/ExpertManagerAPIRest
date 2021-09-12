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
    public class SiniestrosController : ControllerBase
    {
        private ExpertManagerContext _contexto;
        private RepositorioSiniestros _repositorioSiniestros;
        private RepositorioEstados _repositorioEstados;
        private RepositorioAseguradoras _repositorioAseguradoras;
        private RepositorioUsuarios _repositorioUsuarios;
        private RepositorioPeritos _repositorioPeritos;
        private RepositorioDanios _repositorioDanios;

        public SiniestrosController(ExpertManagerContext contexto, RepositorioSiniestros repositorioSiniestros, RepositorioEstados repositorioEstados,
                                    RepositorioAseguradoras repositorioAseguradoras, RepositorioUsuarios repositorioUsuarios, RepositorioPeritos repositorioPeritos,
                                    RepositorioDanios repositorioDanios)
        {
            _contexto = contexto;
            _repositorioSiniestros = repositorioSiniestros;
            _repositorioEstados = repositorioEstados;
            _repositorioAseguradoras = repositorioAseguradoras;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioPeritos = repositorioPeritos;
            _repositorioDanios = repositorioDanios;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int idPerito, int idAseguradora)
        {
            List<Siniestro> siniestros = null;

            if (idPerito == 0)
                siniestros = await _repositorioSiniestros.ObtenerTodos();
            else
                siniestros = await _repositorioSiniestros.ObtenerPorIdPerito(idPerito);

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

        [HttpGet("PeritoNoResponsable")]
        public async Task<ActionResult> ObtenerPorPeritoNoResponsable(int idPerito, int idAseguradora)
        {
            List<Siniestro> siniestros = await _repositorioSiniestros.ObtenerPorIdPerito(idPerito);

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
            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(id);
            Estado estadoCerrado = await _repositorioEstados.ObtenerPorTipo(TipoEstado.Cerrado);

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
        public async Task<ActionResult> ObtenerPorId(int id)
        {
            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(id);

            if (siniestro is null)            
                return NotFound($"No se ha encontrado el siniestro con id {id}");            

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

            return Ok(siniestroVm);
        }

        [HttpPost]        
        public async Task<ActionResult> Create(CrearSiniestroVm crearSiniestroVm)
        {
            Estado estado = await _repositorioEstados.ObtenerPorTipo(TipoEstado.SinValorar);

            if (estado is null)
                return NotFound("No existe el estado con id 2");

            Aseguradora aseguradora = await _repositorioAseguradoras.ObtenerPorId(crearSiniestroVm.IdAseguradora);

            if (aseguradora is null)            
                return NotFound($"No existe la aseguradora con id {crearSiniestroVm.IdAseguradora}");

            Usuario usuarioCreado = await _repositorioUsuarios.ObtenerPorId(crearSiniestroVm.IdUsuarioAlta);

            if (usuarioCreado is null)
                return NotFound($"No existe el usuario con id {crearSiniestroVm.IdUsuarioAlta}");

            SujetoAfectado sujetoAfectado = (SujetoAfectado)crearSiniestroVm.IdSujetoAfectado;

            Usuario perito = await _repositorioPeritos.ObtenerPorId(crearSiniestroVm.IdPerito);

            if (perito is null)            
                return NotFound($"No existe el perito con id {crearSiniestroVm.IdPerito}");

            Danio danio = await _repositorioDanios.ObtenerPorId(crearSiniestroVm.IdDanio);

            if (danio is null)            
                return NotFound($"No existe el daño con id {crearSiniestroVm.IdDanio}");            

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
            bool estaCreado;

            try
            {
                _contexto.Add(siniestro);
                await _contexto.SaveChangesAsync();

                estaCreado = true;
            }
            catch (Exception ex)
            {
                estaCreado = false;
            }

            if (estaCreado)
                return Ok(estaCreado);

            return StatusCode(500, "No se ha podido crear el siniestro");
        }

        [HttpPut("{id}")]        
        public async Task<ActionResult> Edit(int id, SiniestroVm siniestroVm)
        {            
            Estado estado = await _repositorioEstados.ObtenerPorId(siniestroVm.IdEstado);

            if (estado is null)
                return NotFound($"No existe el estado con id {siniestroVm.IdEstado}");

            Aseguradora aseguradora = await _repositorioAseguradoras.ObtenerPorId(siniestroVm.IdAseguradora);

            if (aseguradora is null)
                return NotFound($"No existe la aseguradora con id {siniestroVm.IdAseguradora}");

            SujetoAfectado sujetoAfectado = (SujetoAfectado)siniestroVm.IdSujetoAfectado;

            Usuario perito = await _repositorioPeritos.ObtenerPorId(siniestroVm.IdPerito);

            if (perito is null)
                return NotFound($"No existe el perito con id {siniestroVm.IdPerito}");

            Danio danio = await _repositorioDanios.ObtenerPorId(siniestroVm.IdDanio);

            if (danio is null)
                return NotFound($"No existe el daño con id {siniestroVm.IdDanio}");

            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(id);

            if (siniestro is null)
                return NotFound($"No existe el siniestro con id {id}");

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

            bool estaEditado;

            try
            {                
                _contexto.Update(siniestro);
                await _contexto.SaveChangesAsync();

                estaEditado = true;
            }
            catch (Exception ex)
            {
                estaEditado = false;
            }

            if (estaEditado)
                return Ok(estaEditado);

            return StatusCode(500, "No se ha podido editar el siniestro");
        }

        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(int id)
        {
            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(id);
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
