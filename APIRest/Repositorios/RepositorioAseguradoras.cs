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
    }
}
