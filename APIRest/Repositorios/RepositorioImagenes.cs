﻿using APIRest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRest.Models;
using APIRest.Enumeraciones;
using APIRest.Excepciones;
using System.Net;

namespace APIRest.Repositorios
{
    public class RepositorioImagenes
    {
        private ExpertManagerContext _contexto;

        public RepositorioImagenes(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Archivo>> ObtenerPorIdSiniestro(int id)
        {
            int idTipoArchImagen = (int)TiposArchivo.Imagen;
            List<Archivo> imagenes = await _contexto.Archivos
                                                    .Include(archivo => archivo.Siniestro)
                                                    .Include(archivo => archivo.TipoArchivo)
                                                    .Where(archivo => archivo.Siniestro.Id == id && archivo.TipoArchivo.Id == idTipoArchImagen)
                                                    .ToListAsync();
            if (imagenes is null)
                throw new CodigoErrorHttpException($"No existen imágenes con id de siniestro {id}", HttpStatusCode.NotFound);

            return imagenes;
        }

        public async Task<Archivo> ObtenerPorId(int id)
        {
            Archivo imagen = await _contexto.Archivos                                                                                                      
                                            .Where(archivo => archivo.Id == id)
                                            .FirstOrDefaultAsync();
            if (imagen is null)
                throw new CodigoErrorHttpException($"No existe la imagen con id {id}", HttpStatusCode.NotFound);

            return imagen;
        }

        public async Task Eliminar(Archivo documentacion)
        {
            try
            {
                _contexto.Remove(documentacion);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }                        
        }

        public async Task Guardar(Archivo documentacion)
        {
            try
            {
                _contexto.Add(documentacion);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
