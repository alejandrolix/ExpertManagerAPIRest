using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentacionesController : ControllerBase
    {
        private ExpertManagerContext _contexto;        

        public DocumentacionesController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: DocumentacionesController
        [HttpGet("ObtenerPorIdSiniestro/{idSiniestro}")]
        public async Task<List<DocumentacionVm>> ObtenerPorIdSiniestro(int idSiniestro)
        {
            List<Documentacion> documentaciones = await _contexto.Documentaciones
                                                                 .Where(documentacion => documentacion.SiniestroId == idSiniestro)
                                                                 .ToListAsync();

            List<DocumentacionVm> documentacionesVm = documentaciones.Select(documentacion => new DocumentacionVm()
            {
                Id = documentacion.Id,
                Descripcion = documentacion.Descripcion
            })
            .ToList();

            return documentacionesVm;
        }

        [HttpGet("{id}")]
        public async Task<FileStreamResult> Obtener(int id)
        {
            Documentacion documentacion = await _contexto.Documentaciones
                                                         .FirstOrDefaultAsync(documentacion => documentacion.Id == id);

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
        public async Task<JsonResult> Subir([FromForm] DocumentacionVm documentacionVm)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == documentacionVm.IdSiniestro);
            
            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/documentacion", documentacionVm.Archivo.FileName);
            rutaPdf = rutaPdf.Replace("\\", "/");

            Documentacion documentacion = new Documentacion()
            {
                Descripcion = documentacionVm.Descripcion,                
                Siniestro = siniestro,
                UrlArchivo = rutaPdf
            };

            try
            {
                using (var stream = System.IO.File.Create(rutaPdf))
                {
                    await documentacionVm.Archivo.CopyToAsync(stream);
                }

                _contexto.Add(documentacion);
                await _contexto.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }

        [HttpDelete("{idSiniestro}")]
        public async Task<JsonResult> Eliminar(int idSiniestro)
        {
            Documentacion documentacion = await _contexto.Documentaciones
                                                         .FirstOrDefaultAsync(documentacion => documentacion.Id == idSiniestro);
            try
            {
                if (System.IO.File.Exists(documentacion.UrlArchivo))                
                    System.IO.File.Delete(documentacion.UrlArchivo);                
                
                _contexto.Remove(documentacion);

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
