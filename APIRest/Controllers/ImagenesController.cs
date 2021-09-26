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
    public class ImagenesController : ControllerBase
    {        
        private RepositorioImagenes _repositorioImagenes;
        private RepositorioSiniestros _repositorioSiniestros;
        private RepositorioTiposArchivos _repositorioTiposArchivos;

        public ImagenesController(RepositorioImagenes repositorioImagenes, RepositorioSiniestros repositorioSiniestros, RepositorioTiposArchivos repositorioTiposArchivos)
        {            
            _repositorioImagenes = repositorioImagenes;
            _repositorioSiniestros = repositorioSiniestros;
            _repositorioTiposArchivos = repositorioTiposArchivos;
        }

        [HttpGet("ObtenerPorIdSiniestro/{idSiniestro}")]
        public async Task<ActionResult> ObtenerPorIdSiniestro(int idSiniestro)
        {
            List<Archivo> imagenes = await _repositorioImagenes.ObtenerPorIdSiniestro(idSiniestro);

            if (imagenes is null)
                return NotFound($"No existen imágenes con id de siniestro {idSiniestro}");

            List<ArchivoVm> imagenesVm = imagenes.Select(imagen => new ArchivoVm()
            {
                Id = imagen.Id,
                Descripcion = imagen.Descripcion
            })
            .ToList();

            return Ok(imagenesVm);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Obtener(int id)
        {
            Archivo imagen = await _repositorioImagenes.ObtenerPorId(id);

            if (imagen is null)
                return NotFound($"No existe la imagen con id de siniestro {id}");

            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagen.UrlArchivo);
            rutaPdf = rutaPdf.Replace("\\", "/");

            var memory = new MemoryStream();
            using (var stream = new FileStream(rutaPdf, FileMode.Open))            
                await stream.CopyToAsync(memory);            

            memory.Position = 0;

            string contentType = await ObtenerContentTypeArchivo(id);

            return File(memory, contentType, Path.GetFileName(rutaPdf));
        }

        [HttpGet("ObtenerContentType/{idImagen}")]
        public async Task<string> ObtenerContentTypeArchivo(int idImagen)
        {
            Archivo imagen = await _repositorioImagenes.ObtenerPorId(idImagen);            

            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagen.UrlArchivo);
            rutaPdf = rutaPdf.Replace("\\", "/");

            string extension = Path.GetExtension(rutaPdf).Replace(".", "");
            string contentType;

            if (extension == ".jpeg")
                contentType = "image/jpeg";
            else if (extension == "jpg")
                contentType = "image/jpg";
            else
                contentType = "image/png";

            return contentType;
        }

        [HttpPost]
        public async Task<ActionResult> Subir([FromForm] ArchivoVm imagenVm)
        {
            if (imagenVm.Descripcion is null || imagenVm.Descripcion.Length == 0)
                return StatusCode(500, "La descripción está vacía");

            if (imagenVm.Archivo is null)
                return StatusCode(500, "No se ha seleccionado ningún archivo");

            Siniestro siniestro = await _repositorioSiniestros.ObtenerPorId(imagenVm.IdSiniestro);

            if (siniestro is null)
                return NotFound($"No existe el siniestro con id {imagenVm.IdSiniestro}");

            string rutaPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", imagenVm.Archivo.FileName);
            rutaPdf = rutaPdf.Replace("\\", "/");

            TipoArchivo tipoArchivo = await _repositorioTiposArchivos.ObtenerPorTipo(TiposArchivo.Imagen);
            Archivo imagen = new Archivo()
            {
                Descripcion = imagenVm.Descripcion,
                Siniestro = siniestro,
                UrlArchivo = rutaPdf,
                TipoArchivo = tipoArchivo
            };

            using (var stream = System.IO.File.Create(rutaPdf))
                await imagenVm.Archivo.CopyToAsync(stream);

            try
            {
                await _repositorioImagenes.Guardar(imagen);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al crear la imagen");
            }

            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            Archivo imagen = await _repositorioImagenes.ObtenerPorId(id);

            if (imagen is null)
                return NotFound($"No existe la imagen con id {id}");

            if (System.IO.File.Exists(imagen.UrlArchivo))
            {
                try
                {
                    System.IO.File.Delete(imagen.UrlArchivo);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Ha habido un error al eliminar el archivo");
                }
            }

            try
            {
                await _repositorioImagenes.Eliminar(imagen);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ha habido un error al eliminar la imagen");
            }

            return Ok(true);
        }
    }
}
