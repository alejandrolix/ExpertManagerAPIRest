using APIRest.Context;
using APIRest.Models;
using APIRest.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIRest.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentacionesController : ControllerBase
    {
        private ExpertManagerContext _contexto;
        private IWebHostEnvironment _webHostEnvironment;

        public DocumentacionesController(ExpertManagerContext contexto, IWebHostEnvironment webHostEnvironment)
        {
            _contexto = contexto;
            _webHostEnvironment = webHostEnvironment;
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

            var memory = new MemoryStream();
            using (var stream = new FileStream(rutaPdf, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, "application/pdf", Path.GetFileName(rutaPdf));
        }
    }
}
