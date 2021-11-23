using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.Repositorios;
using APIRest.Excepciones;
using System.Net;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class SiniestrosController : ControllerBase
    {
        private RepositorioSiniestros _repositorioSiniestros;
        private RepositorioEstados _repositorioEstados;
        private RepositorioAseguradoras _repositorioAseguradoras;
        private RepositorioUsuarios _repositorioUsuarios;
        private RepositorioPeritos _repositorioPeritos;
        private RepositorioDanios _repositorioDanios;
        private RepositorioPermisos _repositorioPermisos;

        public SiniestrosController(RepositorioSiniestros repositorioSiniestros, RepositorioEstados repositorioEstados, RepositorioAseguradoras repositorioAseguradoras, RepositorioUsuarios repositorioUsuarios,
                                    RepositorioPeritos repositorioPeritos, RepositorioDanios repositorioDanios, RepositorioPermisos repositorioPermisos)
        {
            _repositorioSiniestros = repositorioSiniestros;
            _repositorioEstados = repositorioEstados;
            _repositorioAseguradoras = repositorioAseguradoras;
            _repositorioUsuarios = repositorioUsuarios;
            _repositorioPeritos = repositorioPeritos;
            _repositorioDanios = repositorioDanios;
            _repositorioPermisos = repositorioPermisos;
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
                ImpValoracionDanios = $"{siniestro.ImpValoracionDanios:C}"
            })
            .ToList();

            return Ok(siniestrosVms);            
        }

        [HttpGet("PeritoNoResponsable")]
        public async Task<ActionResult> ObtenerPorPeritoNoResponsable(int idPerito, int idAseguradora)
        {
            List<Siniestro> siniestros = await _repositorioSiniestros.ObtenerPorIdPerito(idPerito);

            if (idAseguradora != 0)
                siniestros = siniestros.Where(siniestro => siniestro.Aseguradora.Id == idAseguradora)
                                       .ToList();

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
            List<Siniestro> siniestros = await _repositorioSiniestros.ObtenerPorIdPeritoResponsable(idPerito);

            if (idAseguradora != 0)
                siniestros = siniestros.Where(siniestro => siniestro.Aseguradora.Id == idAseguradora)
                                       .ToList();

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

        [HttpPut("Cerrar")]
        public async Task<ActionResult> Cerrar(CerrarSiniestroVm cerrarSiniestroVm)
        {
            await SePuedeCerrar(cerrarSiniestroVm);

            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(cerrarSiniestroVm.IdSiniestro);
            Estado estadoCerrado = await _repositorioEstados.ObtenerPorTipo(TipoEstado.Cerrado);            

            try
            {
                siniestro.Estado = estadoCerrado;
                await _repositorioSiniestros.Actualizar(siniestro);                
            }
            catch (Exception)
            {                
                return StatusCode(500, $"No se ha podido cerrar el siniestro con id {cerrarSiniestroVm.IdSiniestro}");
            }
            
            return Ok(true);            
        }

        private async Task<bool> SePuedeCerrar(CerrarSiniestroVm cerrarSiniestroVm)
        {
            await _repositorioPermisos.ObtenerPorId(cerrarSiniestroVm.IdPermiso);            
            
            bool esPeritoResponsable = _repositorioPermisos.EsPeritoResponsable(cerrarSiniestroVm.IdPermiso);

            if (esPeritoResponsable)                            
                return true;            
            
            bool esPeritoNoResponsable = _repositorioPermisos.EsPeritoNoResponsable(cerrarSiniestroVm.IdPermiso);

            if (!esPeritoNoResponsable)
                throw new CodigoErrorHttpException("No se puede cerrar el siniestro porque el usuario tiene permiso de administración", HttpStatusCode.InternalServerError);            

            bool esImpValoracionDaniosSiniestroMayorQueDelPerito = await EsImpValoracionDaniosSiniestroMayorQueDelPerito(cerrarSiniestroVm.IdPerito, cerrarSiniestroVm.IdSiniestro);

            if (esImpValoracionDaniosSiniestroMayorQueDelPerito)                            
                throw new CodigoErrorHttpException("No se puede cerrar el siniestro porque el importe de valoración de daños supera el establecido al perito", HttpStatusCode.InternalServerError);
            else                
                return true;            
        }

        [HttpGet("EsImpValoracionDaniosSiniestroMayorQuePerito")]
        public async Task<bool> EsImpValoracionDaniosSiniestroMayorQueDelPerito(int idPerito, int idSiniestro)
        {
            Usuario perito = await _repositorioPeritos.ObtenerPorId(idPerito);            
            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(idSiniestro);
            
            if (siniestro.ImpValoracionDanios > perito.ImpRepacionDanios)
                return true;

            return false;
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

        [NonAction]
        public void ValidarSiniestro(SiniestroVm siniestroVm)
        {
            if (siniestroVm.IdAseguradora <= 0)
                throw new Exception("La aseguradora seleccionada no es válida");

            if (siniestroVm.IdUsuarioAlta <= 0)
                throw new Exception("El usuario de alta no es válido");

            List<int> idsSujetoAfectado = Enum.GetValues(typeof(SujetoAfectado)).Cast<int>()
                                                                                .ToList();

            bool existeIdSujetoAfectado = Array.Exists(idsSujetoAfectado.ToArray(), id => id == siniestroVm.IdSujetoAfectado);

            if (!existeIdSujetoAfectado)
                throw new Exception("El sujeto afectado seleccionado no es válido");

            if (siniestroVm.IdPerito <= 0)
                throw new Exception("El perito seleccionado no es válido");

            if (siniestroVm.IdDanio <= 0)
                throw new Exception("El daño seleccionado no es válido");

            if (siniestroVm.Direccion is null || siniestroVm.Direccion.Length == 0)
                throw new Exception("La dirección está vacía");

            if (siniestroVm.Descripcion is null || siniestroVm.Descripcion.Length == 0)
                throw new Exception("La descripción está vacía");            
        }

        [HttpPost]        
        public async Task<ActionResult> Create(SiniestroVm siniestroVm)
        {
            try
            {
                ValidarSiniestro(siniestroVm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            Estado estado = await _repositorioEstados.ObtenerPorTipo(TipoEstado.SinValorar);
            Aseguradora aseguradora = await _repositorioAseguradoras.ObtenerPorId(siniestroVm.IdAseguradora);

            if (aseguradora is null)            
                return NotFound($"No existe la aseguradora con id {siniestroVm.IdAseguradora}");

            Usuario usuarioCreado = await _repositorioUsuarios.ObtenerPorId(siniestroVm.IdUsuarioAlta);

            if (usuarioCreado is null)
                return NotFound($"No existe el usuario con id {siniestroVm.IdUsuarioAlta}");

            SujetoAfectado sujetoAfectado = (SujetoAfectado)siniestroVm.IdSujetoAfectado;

            Usuario perito = await _repositorioPeritos.ObtenerPorId(siniestroVm.IdPerito);

            if (perito is null)            
                return NotFound($"No existe el perito con id {siniestroVm.IdPerito}");

            Danio danio = await _repositorioDanios.ObtenerPorId(siniestroVm.IdDanio);

            if (danio is null)            
                return NotFound($"No existe el daño con id {siniestroVm.IdDanio}");            

            Siniestro siniestro = new Siniestro()
            {
                Estado = estado,
                Aseguradora = aseguradora,
                Direccion = siniestroVm.Direccion,
                Descripcion = siniestroVm.Descripcion,
                UsuarioCreado = usuarioCreado,
                FechaHoraAlta = DateTime.Now,
                SujetoAfectado = sujetoAfectado,
                ImpValoracionDanios = 0.00M,
                Perito = perito,
                Danio = danio
            };            

            try
            {
                await _repositorioSiniestros.Guardar(siniestro);                
            }
            catch (Exception)
            {
                return StatusCode(500, "No se ha podido crear el siniestro");
            }
            
            return Ok(true);            
        }

        [HttpPut("{id}")]        
        public async Task<ActionResult> Edit(int id, SiniestroVm siniestroVm)
        {
            try
            {
                ValidarSiniestro(siniestroVm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

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

            if (siniestroVm.IdEstado == (int)TipoEstado.Valorado)
                siniestro.ImpValoracionDanios = decimal.Parse(siniestroVm.ImpValoracionDanios);
            else
                siniestro.ImpValoracionDanios = 0;

            try
            {
                await _repositorioSiniestros.Actualizar(siniestro);
            }
            catch (Exception)
            {
                return StatusCode(500, "No se ha podido editar el siniestro");
            }
            
            return Ok(true);            
        }

        [HttpDelete("{id}")]        
        public async Task<ActionResult> Delete(int id)
        {
            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(id);            

            if (siniestro is null)                            
                return NotFound($"No existe el siniestro con id {id}");            

            try
            {
                await _repositorioSiniestros.Eliminar(siniestro);                
            }
            catch (Exception)
            {
                return StatusCode(500, $"No se ha podido eliminar el siniestro con id {id}");
            }

            return Ok(true);
        }
    }
}
