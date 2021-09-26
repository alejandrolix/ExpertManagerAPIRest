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
    public class RepositorioTiposArchivos
    {
        private ExpertManagerContext _contexto;

        public RepositorioTiposArchivos(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<TipoArchivo> ObtenerPorTipo(TiposArchivo tipo)
        {
            int idTipoArchivo = (int)tipo;
            TipoArchivo tipoArchivo = await _contexto.TiposArchivo
                                                     .Where(tipoArchivo => tipoArchivo.Id == idTipoArchivo)
                                                     .FirstOrDefaultAsync();
            return tipoArchivo;
        }
    }
}
