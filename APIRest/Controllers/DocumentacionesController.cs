using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APIRest.Repositorios;
using APIRest.Enumeraciones;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentacionesController : ControllerBase
    {        
        private RepositorioDocumentaciones _repositorioDocumentaciones;
        private RepositorioSiniestros _repositorioSiniestros;
        private RepositorioTiposArchivos _repositorioTiposArchivos;

        public DocumentacionesController(RepositorioDocumentaciones repositorioDocumentaciones, RepositorioSiniestros repositorioSiniestros, RepositorioTiposArchivos repositorioTiposArchivos)
        {            
            _repositorioDocumentaciones = repositorioDocumentaciones;
            _repositorioSiniestros = repositorioSiniestros;
            _repositorioTiposArchivos = repositorioTiposArchivos;
        }
        
        [HttpGet("ObtenerPorIdSiniestro/{idSiniestro}")]
        public async Task<ActionResult> ObtenerPorIdSiniestro(int idSiniestro)
        {
            List<Archivo> documentaciones = await _repositorioDocumentaciones.ObtenerPorIdSiniestro(idSiniestro);

            if (documentaciones is null)
                return NotFound($"No existen documentaciones con id de siniestro {idSiniestro}");

            List<ArchivoVm> documentacionesVm = documentaciones.Select(documentacion => new ArchivoVm()
            {
                Id = documentacion.Id,
                Descripcion = documentacion.Descripcion
            })
            .ToList();

            return Ok(documentacionesVm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Obtener(int id)
        {
            Archivo documentacion = await _repositorioDocumentaciones.ObtenerPorId(id);

            if (documentacion is null)
                return NotFound($"No existe la documentación con id {id}");

            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documentacion.UrlArchivo);
            rutaPdf = rutaPdf.Replace("\\", "/");

            var memory = new MemoryStream();
            using (var stream = new FileStream(rutaPdf, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(rutaPdf));
        }

        [HttpPost]
        public async Task<ActionResult> Subir([FromForm] ArchivoVm documentacionVm)
        {
            if (documentacionVm.Descripcion is null || documentacionVm.Descripcion.Length == 0)
                return StatusCode(500, "La descripción está vacía");

            if (documentacionVm.Archivo is null)
                return StatusCode(500, "No se ha seleccionado ningún archivo");

            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(documentacionVm.IdSiniestro);

            if (siniestro is null)
                return NotFound($"No existe el siniestro con id {documentacionVm.IdSiniestro}");            
            
            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documentacion", documentacionVm.Archivo.FileName);
            rutaPdf = rutaPdf.Replace("\\", "/");
            
            TipoArchivo tipoArchivo = await _repositorioTiposArchivos.ObtenerPorTipo(TiposArchivo.Documentacion);
            Archivo documentacion = new Archivo()
            {
                Descripcion = documentacionVm.Descripcion,
                Siniestro = siniestro,
                UrlArchivo = rutaPdf,
                TipoArchivo = tipoArchivo
            };

            using (var stream = System.IO.File.Create(rutaPdf))            
                await documentacionVm.Archivo.CopyToAsync(stream);            

            try
            {
                await _repositorioDocumentaciones.Guardar(documentacion);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al crear la documentación");
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            Archivo documentacion = await _repositorioDocumentaciones.ObtenerPorId(id);

            if (documentacion is null)
                return NotFound($"No existe la documentación con id {id}");

            if (System.IO.File.Exists(documentacion.UrlArchivo))
            {
                try
                {
                    System.IO.File.Delete(documentacion.UrlArchivo);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Ha habido un error al eliminar el archivo");
                }                
            }

            try
            {
                await _repositorioDocumentaciones.Eliminar(documentacion);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al eliminar la documentación");
            }

            return Ok(true);
        }
    }
}
