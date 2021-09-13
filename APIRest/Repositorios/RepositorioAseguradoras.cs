using APIRest.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRest.Models;

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
            return aseguradora;
        }

        public async Task<List<Aseguradora>> ObtenerTodas()
        {
            List<Aseguradora> aseguradoras = await _contexto.Aseguradoras
                                                            .ToListAsync();
            return aseguradoras;
        }
    }
}
