using APIRest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRest.Models;
using APIRest.Excepciones;
using System.Net;

namespace APIRest.Repositorios
{
    public class RepositorioAseguradoras
    {
        private ExpertManagerContext _contexto;

        public RepositorioAseguradoras(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<Aseguradora> ObtenerPorId(int id)
        {
            Aseguradora aseguradora = await _contexto.Aseguradoras
                                                     .Where(aseguradora => aseguradora.Id == id)
                                                     .FirstOrDefaultAsync();
            if (aseguradora is null)
                throw new CodigoErrorHttpException($"No existe la aseguradora con id {id}", HttpStatusCode.NotFound);

            return aseguradora;
        }

        public async Task<List<Aseguradora>> ObtenerTodas()
        {
            List<Aseguradora> aseguradoras = await _contexto.Aseguradoras
                                                            .ToListAsync();
            if (aseguradoras is null)
                throw new CodigoErrorHttpException("No existen aseguradoras", HttpStatusCode.NotFound);

            return aseguradoras;
        }

        public async Task Eliminar(Aseguradora aseguradora)
        {
            try
            {
                _contexto.Remove(aseguradora);
                await _contexto.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
