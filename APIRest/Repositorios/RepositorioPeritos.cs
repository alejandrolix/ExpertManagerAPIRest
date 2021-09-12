using APIRest.Context;
using APIRest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIRest.Repositorios
{
    public class RepositorioPeritos
    {
        private ExpertManagerContext _contexto;

        public RepositorioPeritos(ExpertManagerContext contexto)
        {
            _contexto = contexto;
        }
    }
}
