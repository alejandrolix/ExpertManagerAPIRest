using APIRest.Context;
using APIRest.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioEstados
    {
        private ExpertManagerContext _contexto;

        public RepositorioEstados(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }

        public enum Tipo
        {
            Procesando = 1,
            SinValorar = 2,
            Valorado = 3,
            Cerrado = 4
        }

        public async Task<Estado> ObtenerPorTipo(Tipo tipo)
        {
            int idEstado = (int)tipo;
            Estado estado = await _contexto.Estados
                                           .Where(estado => estado.Id == idEstado)
                                           .FirstOrDefaultAsync();
            return estado;
        }
    }
}
