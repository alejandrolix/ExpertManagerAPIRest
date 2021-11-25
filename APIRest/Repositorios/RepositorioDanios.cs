using APIRest.Context;
using APIRest.Excepciones;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace APIRest.Repositorios
{
    public class RepositorioDanios
    {
        private ExpertManagerContext _contexto;

        public RepositorioDanios(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Danio> ObtenerPorId(int id)
        {
            Danio danio = await _contexto.Danios
                                         .Where(danio => danio.Id == id)
                                         .FirstOrDefaultAsync();
            if (danio is null)
                throw new CodigoErrorHttpException($"No existe el daño con id {id}", HttpStatusCode.NotFound);

            return danio;
        }

        public async Task<List<Danio>> ObtenerTodos()
        {
            List<Danio> danios = await _contexto.Danios
                                                .ToListAsync();
            if (danios is null)
                throw new CodigoErrorHttpException("No existen daños", HttpStatusCode.NotFound);

            return danios;
        }
    }
}
