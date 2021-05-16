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
    public class ImagenesController : ControllerBase
    {
        private ExpertManagerContext _contexto;        

        public ImagenesController(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        // GET: ImagenesController
        [HttpGet("ObtenerPorIdSiniestro/{idSiniestro}")]
        public async Task<List<ImagenVm>> ObtenerPorIdSiniestro(int idSiniestro)
        {
            List<Imagen> imagenes = await _contexto.Imagenes
                                                    .Where(imagen => imagen.SiniestroId == idSiniestro)
                                                    .ToListAsync();

            List<ImagenVm> imagenesVm = imagenes.Select(imagen => new ImagenVm()
            {
                Id = imagen.Id,
                Descripcion = imagen.Descripcion
            })
            .ToList();

            return imagenesVm;
        }

        [HttpGet("{id}")]
        public async Task<FileStreamResult> Obtener(int id)
        {
            Imagen imagen = await _contexto.Imagenes
                                           .FirstOrDefaultAsync(imagen => imagen.Id == id);

            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagen.UrlArchivo);
            rutaPdf = rutaPdf.Replace("\\", "/");

            var memory = new MemoryStream();
            using (var stream = new FileStream(rutaPdf, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            string extension = Path.GetExtension(rutaPdf).Replace(".", "");
            string contentType;

            if (extension == ".jpeg")
                contentType = "image/jpeg";
            else if (extension == "jpg")
                contentType = "image/jpg";
            else
                contentType = "image/png";

            return File(memory, contentType, Path.GetFileName(rutaPdf));
        }

        [HttpPost]
        public async Task<JsonResult> Subir([FromForm] ImagenVm imagenVm)
        {
            Siniestro siniestro = await _contexto.Siniestros
                                                 .FirstOrDefaultAsync(siniestro => siniestro.Id == imagenVm.IdSiniestro);
            
            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", imagenVm.Imagen.FileName);
            rutaPdf = rutaPdf.Replace("\\", "/");

            Imagen imagen = new Imagen()
            {
                Descripcion = imagenVm.Descripcion,                
                Siniestro = siniestro,
                UrlArchivo = rutaPdf
            };

            try
            {
                using (var stream = System.IO.File.Create(rutaPdf))
                {
                    await imagenVm.Imagen.CopyToAsync(stream);
                }

                _contexto.Add(imagen);
                await _contexto.SaveChangesAsync();

                return new JsonResult(true);
            }
            catch (Exception ex)
            {
                return new JsonResult(false);
            }
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Eliminar(int id)
        {
            Imagen imagen = await _contexto.Imagenes
                                           .FirstOrDefaultAsync(imagen => imagen.Id == id);
            try
            {
                if (System.IO.File.Exists(imagen.UrlArchivo))                
                    System.IO.File.Delete(imagen.UrlArchivo);                
                
                _contexto.Remove(imagen);

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
