using APIRest.Context;
using APIRest.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public enum TipoEstado
    {
        Procesando = 1,
        SinValorar = 2,
        Valorado = 3,
        Cerrado = 4
    }

    public class RepositorioEstados
    {
        private ExpertManagerContext _contexto;

        public RepositorioEstados(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }        

        public async Task<Estado> ObtenerPorTipo(TipoEstado tipo)
        {
            int idEstado = (int)tipo;
            Estado estado = await _contexto.Estados
                                           .Where(estado => estado.Id == idEstado)
                                           .FirstOrDefaultAsync();
            return estado;
        }
        
        public async Task<Estado> ObtenerPorId(int id)
        {            
            Estado estado = await _contexto.Estados
                                           .Where(estado => estado.Id == id)
                                           .FirstOrDefaultAsync();
            return estado;
        }
    }
}
