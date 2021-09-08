using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRest.Context;
using Microsoft.EntityFrameworkCore;

namespace APIRest.Helpers
{
    public class Estado
    {
        private static ExpertManagerContext _contexto;

        public Estado(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public static async Task<Models.Estado> ObtenerPorId(int id)
        {
            Models.Estado estado = await _contexto.Estados
                                                  .Where(estado => estado.Id == id)
                                                  .FirstOrDefaultAsync();
            return estado;
        }
    }
}
