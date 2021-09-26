using APIRest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRest.Models;
using APIRest.Enumeraciones;

namespace APIRest.Repositorios
{
    public class RepositorioDocumentaciones
    {
        private ExpertManagerContext _contexto;

        public RepositorioDocumentaciones(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<Archivo>> ObtenerPorIdSiniestro(int id)
        {
            int idTipoArchDocumentacion = (int)TiposArchivo.Documentacion;
            List<Archivo> documentaciones = await _contexto.Archivos
                                                           .Include(archivo => archivo.Siniestro)
                                                           .Include(archivo => archivo.TipoArchivo)
                                                           .Where(archivo => archivo.Siniestro.Id == id && archivo.TipoArchivo.Id == idTipoArchDocumentacion)
                                                           .ToListAsync();
            return documentaciones;
        }

        public async Task<Archivo> ObtenerPorId(int id)
        {
            Archivo documentacion = await _contexto.Archivos                                                                                                      
                                                   .Where(archivo => archivo.Id == id)
                                                   .FirstOrDefaultAsync();
            return documentacion;
        }
    }
}
